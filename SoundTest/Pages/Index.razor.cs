using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using static SoundTest.Constants;

namespace SoundTest.Pages
{
    public partial class Index
    {
        private Types type;
        private int frequency;

        protected override void OnInitialized()
        {
            Dictionary<string, StringValues> queryString = QueryHelpers.ParseQuery(Navigation.ToAbsoluteUri(Navigation.Uri).Query);

            if (!queryString.TryGetValue(nameof(type), out StringValues typeString) || !Enum.TryParse(typeString, out type))
            {
                type = DefaultType;
            }

            if (!queryString.TryGetValue(nameof(frequency), out StringValues frequencyString) || !int.TryParse(frequencyString, out frequency))
            {
                frequency = DefaultFrequency;
            }
        }
    }
}
