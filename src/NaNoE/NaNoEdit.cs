using System;
using System.Collections.Generic;

/// Features that need to be implimented here
/// -=- Editing helpers
///   -> Spelling errors
///   -> Repeated words
///   -> Grammar errors (not completely 'errors')
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

            // Spelling
            var splt = para.Split(' ');
            for (int i = 0; i < splt.Length; i++)
            {
                if (!SpellCheck(splt[i])) ans.Add("{" + i.ToString() + "} Spelling Error: " + splt[i]);
            }

            // [ replace 'to be' and 'to have' ]
            if (para.Contains(" to be.")
             || para.Contains(" to be,")
             || para.Contains(" to be ")
             || para.Contains(" to have.")
             || para.Contains(" to have,")
             || para.Contains(" to have ")) ans.Add("[" + para.IndexOf("to ") +"]replace 'to be' and 'to have' with something");
            // [ ly = 'He quickly ran across the park.' fixed to 'He darted across the park.' ]
            if (para.Contains("ly ")
             || para.Contains("ly,")
             || para.Contains("ly."))           ans.Add("[" + para.IndexOf("ly") + "]replace '-ly' words with descriptions");
            /// [ ing = 'I turned and Mary was glaring at me.' fixed to 'I turned and Mary glared.' ]
            if (para.Contains("ing ")
             || para.Contains("ing,")
             || para.Contains("ing."))          ans.Add("[" + para.IndexOf("ing") + "]replace '-ing' words with minimal words");
            // [ -> Begin, begins, began, beginning, start, starts, started, starting = 'he started to run' fixed by 'he ran' ]
            if (para.Contains("begin"))         ans.Add("[" + para.IndexOf("begin") +"]Rather don't use 'begin', simplify it");
            if (para.Contains("begins"))        ans.Add("[" + para.IndexOf("begins") +"]Rather don't use 'begins', simplify it");
            if (para.Contains("began"))         ans.Add("[" + para.IndexOf("began") +"]Rather don't use 'began', simplify it");
            if (para.Contains("beginning"))     ans.Add("[" + para.IndexOf("beginning") +"]Rather don't use 'beginning', simplify it");
            if (para.Contains("start"))         ans.Add("[" + para.IndexOf("start") +"]Rather don't use 'start', simplify it");
            if (para.Contains("starts"))        ans.Add("[" + para.IndexOf("starts") +"]Rather don't use 'starts', simplify it");
            if (para.Contains("started"))       ans.Add("[" + para.IndexOf("started") +"]Rather don't use 'started', simplify it");
            if (para.Contains("starting"))      ans.Add("[" + para.IndexOf("starting") + "]Rather don't use 'starting', simplify it");
            // [ when, then, suddenly, immediately, always, often, already, finally = 'I immediately ran through the door.' fixed to 'I ran through the door.' ]
            if (para.Contains("when"))          ans.Add("[" + para.IndexOf("when") +"]Rather minify use 'when', make literal");
            if (para.Contains("then"))          ans.Add("[" + para.IndexOf("then") +"]Rather minify use 'then', make literal");
            if (para.Contains("suddenly"))      ans.Add("[" + para.IndexOf("suddenly") +"]Rather minify use 'suddenly', make literal");
            if (para.Contains("immediately"))   ans.Add("[" + para.IndexOf("immediately") +"]Rather minify use 'immediately', make literal");
            if (para.Contains("always"))        ans.Add("[" + para.IndexOf("always") +"]Rather minify use 'always', make literal");
            if (para.Contains("often"))         ans.Add("[" + para.IndexOf("often") +"]Rather minify use 'often', make literal");
            if (para.Contains("already"))       ans.Add("[" + para.IndexOf("already") +"]Rather minify use 'already', make literal");
            if (para.Contains("finally"))       ans.Add("[" + para.IndexOf("finally") + "]Rather minify use 'finally', make literal");
            // [ See, Look, Hear, Know, Realize, Wonder, Decided, Notice, Feel, Remember, Think, That, Of, Really, Very, Down, Up, Then, Start, begin, Just = reword ]
            if (para.Contains("see"))           ans.Add("[" + para.IndexOf("see") +"] Rather minify use 'see', make literal, cant think of another way to mention this TBH");
            if (para.Contains("look"))          ans.Add("[" + para.IndexOf("look") +"] Rather minify use 'look', make literal, cant think of another way to mention this TBH");
            if (para.Contains("hear"))          ans.Add("[" + para.IndexOf("hear") +"] Rather minify use 'hear', make literal, cant think of another way to mention this TBH");
            if (para.Contains("know"))          ans.Add("[" + para.IndexOf("know") +"] Rather minify use 'know', make literal, cant think of another way to mention this TBH");
            if (para.Contains("realize"))       ans.Add("[" + para.IndexOf("realize") +"] Rather minify use 'realize', make literal, cant think of another way to mention this TBH");
            if (para.Contains("realise"))       ans.Add("[" + para.IndexOf("realise") +"] Rather minify use 'realise', make literal, cant think of another way to mention this TBH");
            if (para.Contains("wonder"))        ans.Add("[" + para.IndexOf("wonder") +"] Rather minify use 'wonder', make literal, cant think of another way to mention this TBH");
            if (para.Contains("decided"))       ans.Add("[" + para.IndexOf("decided") +"] Rather minify use 'decided', make literal, cant think of another way to mention this TBH");
            if (para.Contains("notice"))        ans.Add("[" + para.IndexOf("notice") +"] Rather minify use 'notic', make literal, cant think of another way to mention this TBH");
            if (para.Contains("feel"))          ans.Add("[" + para.IndexOf("feel") +"] Rather minify use 'feel', make literal, cant think of another way to mention this TBH");
            if (para.Contains("remember"))      ans.Add("[" + para.IndexOf("remember") +"] Rather minify use 'remember', make literal, cant think of another way to mention this TBH");
            if (para.Contains("think"))         ans.Add("[" + para.IndexOf("think") +"] Rather minify use 'think', make literal, cant think of another way to mention this TBH");
            if (para.Contains("that"))          ans.Add("[" + para.IndexOf("that") +"] Rather minify use 'that', make literal, cant think of another way to mention this TBH");
            if (para.Contains("of"))            ans.Add("[" + para.IndexOf("of") +"] Rather minify use 'of', make literal, cant think of another way to mention this TBH");
            if (para.Contains("really"))        ans.Add("[" + para.IndexOf("really") +"] Rather minify use 'really', make literal, cant think of another way to mention this TBH");
            if (para.Contains("very"))          ans.Add("[" + para.IndexOf("very") +"] Rather minify use 'very', make literal, cant think of another way to mention this TBH");
            if (para.Contains("down"))          ans.Add("[" + para.IndexOf("down") +"] Rather minify use 'down', make literal, cant think of another way to mention this TBH");
            if (para.Contains("up"))            ans.Add("[" + para.IndexOf("up") +"] Rather minify use 'up', make literal, cant think of another way to mention this TBH");
            if (para.Contains("then"))          ans.Add("[" + para.IndexOf("then") +"] Rather minify use 'then', make literal, cant think of another way to mention this TBH");
            if (para.Contains("start"))         ans.Add("[" + para.IndexOf("start") +"] Rather minify use 'start', make literal, cant think of another way to mention this TBH");
            if (para.Contains("begin"))         ans.Add("[" + para.IndexOf("begin") +"] Rather minify use 'begin', make literal, cant think of another way to mention this TBH");
            if (para.Contains("just"))          ans.Add("[" + para.IndexOf("just") + "] Rather minify use 'just', make literal, cant think of another way to mention this TBH");

            // Debug
            //ans.Add("This isnt a problem. Just a test.");
            // =========================================================
            // = End Checks                                            =
            // =========================================================
            return ans;
        }

        private static bool SpellCheck(string v)
        {
            throw new NotImplementedException();
        }
    }
}
