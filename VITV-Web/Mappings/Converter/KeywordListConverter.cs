using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VITV.Data.Models;
using VITV.Data.Repositories;
using VITV.Web.ViewModels;

namespace VITV.Web.Mappings
{
    public class KeywordListConverter : ITypeConverter<string, ICollection<Keyword>>
    {
        private readonly IKeywordRepository _keywordRepository;

        public KeywordListConverter(IKeywordRepository keywordRepository)
        {
            _keywordRepository = keywordRepository;
        }

        public ICollection<Keyword> Convert(ResolutionContext context)
        {
            var keywordStr = context.SourceValue as string;
            if (keywordStr == null)
            {
                return null;
            }

            return StringToKeywordList(keywordStr);
        }

        private List<Keyword> StringToKeywordList(string keywordStr)
        {
            List<Keyword> keywords = new List<Keyword>();
            String[] sKeywords = keywordStr.Split(new char[] { ',' });
            foreach (string kw in sKeywords)
            {
                var kwKey = kw.Trim();
                Keyword keyword = _keywordRepository.FindValue(kwKey);
                if (keyword == null)
                {
                    keyword = new Keyword
                    {
                        Value = kwKey
                    };
                    _keywordRepository.InsertOrUpdate(keyword);
                    _keywordRepository.Save();
                }
                keywords.Add(keyword);

            }
            return keywords;
        }

    }
}