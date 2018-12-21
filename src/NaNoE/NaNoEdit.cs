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
            para = para.ToLower();

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
            // [ -> Begin, begins, began, beginning, start, starts, started, starting = 'he started to run' fixed by 'he ran' ]
            if (para.Contains("begin"))         ans.Add("Rather don't use 'begin', simplify it");
            if (para.Contains("begins"))        ans.Add("Rather don't use 'begins', simplify it");
            if (para.Contains("began"))         ans.Add("Rather don't use 'began', simplify it");
            if (para.Contains("beginning"))     ans.Add("Rather don't use 'beginning', simplify it");
            if (para.Contains("start"))         ans.Add("Rather don't use 'start', simplify it");
            if (para.Contains("starts"))        ans.Add("Rather don't use 'starts', simplify it");
            if (para.Contains("started"))       ans.Add("Rather don't use 'started', simplify it");
            if (para.Contains("starting"))      ans.Add("Rather don't use 'starting', simplify it");
            // [ when, then, suddenly, immediately, always, often, already, finally = 'I immediately ran through the door.' fixed to 'I ran through the door.' ]
            if (para.Contains("when"))          ans.Add("Rather minify use 'when', make literal");
            if (para.Contains("then"))          ans.Add("Rather minify use 'then', make literal");
            if (para.Contains("suddenly"))      ans.Add("Rather minify use 'suddenly', make literal");
            if (para.Contains("immediately"))   ans.Add("Rather minify use 'immediately', make literal");
            if (para.Contains("always"))        ans.Add("Rather minify use 'always', make literal");
            if (para.Contains("often"))         ans.Add("Rather minify use 'often', make literal");
            if (para.Contains("already"))       ans.Add("Rather minify use 'already', make literal");
            if (para.Contains("finally"))       ans.Add("Rather minify use 'finally', make literal");


            // Debug
            //ans.Add("This isnt a problem. Just a test.");
            // =========================================================
            // = End Checks                                            =
            // =========================================================
            return ans;
        }
    }
}
