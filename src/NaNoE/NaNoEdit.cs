using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// Features that need to be implimented here
/// -=- Editing helpers
///   -> Spelling errors
///   -> Repeated words
///   -> Grammar errors (not completely 'errors')
///     -> Begin, begins, began, beginning, start, starts, started, starting = 'he started to run' fixed by 'he ran'
///     -> when, then, suddenly, immediately, always, often, already, finally = 'I immediately ran through the door.' fixed to 'I ran through the door.'
///     -> See, Look, Hear, Know, Realize, Wonder, Decided, Notice, Feel, Remember, Think, That, Of, Really, Very, Down, Up, Then, Start, begin, Just = reword
///     -> The = 'the rain spattered on the night sky' fixed with 'rain spattered on the night sky'
///     -> That vs. Who = sometimes 'who' feels better than 'that'
///     -> almost, rather, somewhat = 'we has rather large' fixed with 'he towered over the three men'
///   -> Redundant phrases
///     -> Final outcome, actual fact, added bonus, close proximity, protest against, repeat again, armed gunman
///   -> past/present - double check things are current tense
///   -> Perhaps make it show larger text when clicking on listbox item?

namespace NaNoE
{
    static class NaNoEdit
    {
        /// <summary>
        /// A simple way to precess the paragraphs you make for a novel.
        /// </summary>
        /// <param name="para">Paragraph to check</param>
        /// <returns>List of issues, empty list = no problems</returns>
        public static List<string> Process(string para)
        {
            var ans = new List<string>();
            // =========================================================
            // = Checks added                                          =
            // =========================================================

            // [ replace 'to be' and 'to have' ]
            if (para.Contains(" to be.")
             || para.Contains(" to be,")
             || para.Contains(" to be ")
             || para.Contains(" to have.")
             || para.Contains(" to have,")
             || para.Contains(" to have "))     ans.Add("replace 'to be' and 'to have' with something");
            // [ ly = 'He quickly ran across the park.' fixed to 'He darted across the park.' ]
            if (para.Contains("ly ")
             || para.Contains("ly,")
             || para.Contains("ly."))           ans.Add("replace '-ly' words with descriptions");
            /// [ ing = 'I turned and Mary was glaring at me.' fixed to 'I turned and Mary glared.' ]
            if (para.Contains("ing ")
             || para.Contains("ing,")
             || para.Contains("ing."))          ans.Add("replace '-ing' words with minimal words");



            // Debug
            //ans.Add("This isnt a problem. Just a test.");
            // =========================================================
            // = End Checks                                            =
            // =========================================================
            return ans;
        }
    }
}
