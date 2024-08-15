using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Models
{
    public class ChineseVocabulary
    {
        public int id { get; set; }
        public string simplified { get; set; }
        public string traditional { get; set; }
        public string pinyin { get; set; }
        public string pinyin_tones { get; set; }
        public string translation { get; set; }
        public byte hsk_level { get; set; }
        public string translation_indo { get; set; }
    }
}
