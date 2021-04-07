using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Tanji.Habbo;
using Tanji.Controls;

namespace Tanji.Services.Options
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class SettingsPage : NotifiablePage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        static string Revision;
        private void GenerateMessageHashesBtn_Click(object sender, EventArgs e)
        {
#if DEBUG
            FindForm().WindowState = FormWindowState.Minimized;
#endif
            Revision ??= "HabboAir";
            string swfPath = $"Revisions/{Revision}.swf";
            var swf = new FlashGame(swfPath);
            swf.Disassemble();
            swf.GenerateMessageHashes("Messages.ini");

            var habbo = new Habbo() { Revision = swf.Revision };
            var hashes = new Habbo() { Revision = swf.Revision };
            var headers = new Habbo() { Revision = swf.Revision };

            if (!Directory.Exists(@"Habbo/Outgoing")) Directory.CreateDirectory("Habbo/Outgoing");
            if (!Directory.Exists(@"Habbo/Incoming")) Directory.CreateDirectory("Habbo/Incoming");

            Array.ForEach(Directory.GetFiles(@"Habbo/Outgoing"), delegate (string path) { File.Delete(path); });
            Array.ForEach(Directory.GetFiles(@"Habbo/Incoming"), delegate (string path) { File.Delete(path); });

            foreach (var msg in swf.Messages)
            {
                // Habbo
                var h = new HId()
                {
                    Hash = msg.Key,
                    Name = msg.Value.Name.name,
                    IsOutgoing = msg.Value.IsOutgoing,
                    Structure = msg.Value.Structure
                };

                // Habbo.json
                var hId = (HId)h.Clone();
                hId.Id = msg.Value.Id;
                hId.ClassName = msg.Value.MessageClass.QName.Name;
                if (msg.Value.ParserClass != null) hId.ParserName = msg.Value.ParserClass.QName.Name;

                // Hashes.json
                var hIdHash = new HId()
                {
                    Hash = msg.Key,
                    Name = msg.Value.Name.name
                };

                // Headers.json
                var hIdHeader = new HId()
                {
                    Id = msg.Value.Id,
                    Hash = msg.Key,
                    Name = msg.Value.Name.name,
                    ClassName = msg.Value.MessageClass.QName.Name
                };

                string subpath;
                if (msg.Value.IsOutgoing)
                {
                    subpath = "Outgoing";
                    habbo.Outgoing.Add(hId);
                    hashes.Outgoing.Add(hIdHash);
                    headers.Outgoing.Add(hIdHeader);
                }
                else
                {
                    subpath = "Incoming";
                    habbo.Incoming.Add(hId);
                    hashes.Incoming.Add(hIdHash);
                    headers.Incoming.Add(hIdHeader);
                }

                string path;
                if (msg.Value.Name.name == null)
                {
                    path = $@"Habbo/{subpath}/z_{msg.Key}.json";
                }
                else path = $@"Habbo/{subpath}/{msg.Value.Name.name}.json";

                string h_str = JsonConvert.SerializeObject(h, new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                var h_json = JObject.Parse(h_str);
                File.WriteAllText(path, h_json.ToString());
            }

            string habbo_str = JsonConvert.SerializeObject(habbo, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore });
            string hashes_str = JsonConvert.SerializeObject(hashes, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore });
            string headers_str = JsonConvert.SerializeObject(headers, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore });

            var habbo_json = JObject.Parse(habbo_str);
            var hashes_json = JObject.Parse(hashes_str);
            var headers_json = JObject.Parse(headers_str);

            File.WriteAllText(@"Hashes.json", hashes_json.ToString());
            File.WriteAllText(@"Headers.json", headers_json.ToString());
            File.WriteAllText(@"Habbo/Habbo.json", habbo_json.ToString());

            MessageBox.Show("Generated", "Tanji ~ Info!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        class MessageInfo
        {
            public ushort Id { get; set; }
            public string Hash { get; set; }
            public string Name { get; set; }
        }

        class Habbo
        {
            public string Revision;
            public List<HId> Outgoing = new List<HId>();
            public List<HId> Incoming = new List<HId>();
        }

        class HId : ICloneable
        {
            public object Clone()
            {
                return MemberwiseClone();
            }
            public short Id { get; set; }
            public string Hash { get; set; }
            public bool IsOutgoing { get; set; }
            public string Name { get; set; }
            public string Structure { get; set; }
            public string ClassName { get; set; }
            public string ParserName { get; set; }
            public List<HReference> References { get; set; }
        }

        public class HReference
        {
            public int ClassRank { get; set; }
            public int GroupCount { get; set; }
            public int MethodRank { get; set; }
            public int InstructionRank { get; set; }
        }
    }
}