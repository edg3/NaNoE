using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PlatformSpellCheck;

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
        private static SpellChecker spellChecker = new PlatformSpellCheck.SpellChecker();
        private static Dictionary<string, string> tenseDict = new Dictionary<string, string>();

        /// <summary>
        /// Load the file, generate the array, remove the file itself from memory
        /// </summary>
        public static void Init()
        {
            var tenses = File.OpenRead("grammarhelp.txt");
            using (StreamReader tensesReader = new StreamReader(tenses))
            {
                string line = null;
                while ((line = tensesReader.ReadLine()) != null)
                {
                    var tenseSplit = line.Split(',');
                    tenseDict.Add(tenseSplit[1], tenseSplit[0]);
                }
            }

            tenses.Close();
        }

        /// <summary>
        /// A simple way to precess the paragraphs you make for a novel.
        /// </summary>
        /// <param name="para">Paragraph to check</param>
        /// <returns>List of issues, empty list = no problems</returns>
        public static List<string> Process(string para)
        {
            var ans = new List<string>();
            para = para.ToLower();
            para = para.Replace(",", " ")
                       .Replace(".", " ")
                       .Replace(";", " ");

            // =========================================================
            // = Checks added                                          =
            // =========================================================

            // Spelling
            var splt = para.Split(' ');
            for (int i = 0; i < splt.Length; i++)
            {
                //var whichUsed = new string(splt[i].ToCharArray(0,splt[i].Length).Where(c => !char.IsSeparator(c)).ToArray());
                var whichUsed = splt[i];
                while (whichUsed.EndsWith(".") || whichUsed.EndsWith(",") || whichUsed.EndsWith(";") || whichUsed.EndsWith(":") || whichUsed.EndsWith(" "))
                {
                    whichUsed = whichUsed.Remove(whichUsed.Length - 1);
                }
                // if (!SpellCheck(whichUsed)) ans.Add("{" + i.ToString() + "} Spelling Error: " + splt[i]);
            }

            // Side note: this is definitely going to find things that shouldn't be in this - like portions of other words. "is" is in "sister" for example
            // [ replace 'to be' and 'to have' ]
            if (para.Contains(" to be ")
             || para.Contains(" to have ")) ans.Add("[" + para.IndexOf("to ") +"]replace 'to be' and 'to have' with something");
            // [ ly = 'He quickly ran across the park.' fixed to 'He darted across the park.' ]
            if (para.Contains("ly "))           ans.Add("[" + para.IndexOf("ly") + "]replace '-ly' more descriptive: e.g. not 'her eyes were deadly', rather 'with an evil glare she looked at me'");
            /// [ ing = 'I turned and Mary was glaring at me.' fixed to 'I turned and Mary glared.' ]
            if (para.Contains("ing ")
             || para.Contains("ing,")
             || para.Contains("ing."))          ans.Add("[" + para.IndexOf("ing") + "]replace '-ing' words with minimal words. e.g. 'she is running daily now' with 'she runs every morning now'");
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
            if (para.Contains("see"))           ans.Add("[" + para.IndexOf("see") +"] Rather minify use 'see' ");
            if (para.Contains("look"))          ans.Add("[" + para.IndexOf("look") +"] Rather minify use 'look' ");
            if (para.Contains("hear"))          ans.Add("[" + para.IndexOf("hear") +"] Rather minify use 'hear' ");
            if (para.Contains("know"))          ans.Add("[" + para.IndexOf("know") +"] Rather minify use 'know' ");
            if (para.Contains("realize"))       ans.Add("[" + para.IndexOf("realize") +"] Rather minify use 'realize' ");
            if (para.Contains("realise"))       ans.Add("[" + para.IndexOf("realise") +"] Rather minify use 'realise' ");
            if (para.Contains("wonder"))        ans.Add("[" + para.IndexOf("wonder") +"] Rather minify use 'wonder' ");
            if (para.Contains("decided"))       ans.Add("[" + para.IndexOf("decided") +"] Rather minify use 'decided' ");
            if (para.Contains("notice"))        ans.Add("[" + para.IndexOf("notice") +"] Rather minify use 'notic' ");
            if (para.Contains("feel"))          ans.Add("[" + para.IndexOf("feel") +"] Rather minify use 'feel' ");
            if (para.Contains("remember"))      ans.Add("[" + para.IndexOf("remember") +"] Rather minify use 'remember' ");
            if (para.Contains("think"))         ans.Add("[" + para.IndexOf("think") +"] Rather minify use 'think' ");
            if (para.Contains("that"))          ans.Add("[" + para.IndexOf("that") +"] Rather minify use 'that' ");
            if (para.Contains("of"))            ans.Add("[" + para.IndexOf("of") +"] Rather minify use 'of' ");
            if (para.Contains("really"))        ans.Add("[" + para.IndexOf("really") +"] Rather minify use 'really', make literal, e.g. 'the swimmer really performed admirably' with 'the swimmer performed admirably'. Slacker descriptive");
            if (para.Contains("very"))          ans.Add("[" + para.IndexOf("very") +"] Rather minify use 'very'H");
            if (para.Contains("down"))          ans.Add("[" + para.IndexOf("down") +"] Rather minify use 'down'");
            if (para.Contains("up"))            ans.Add("[" + para.IndexOf("up") +"] Rather minify use 'up'");
            if (para.Contains("then"))          ans.Add("[" + para.IndexOf("then") +"] Rather minify use 'then'");
            if (para.Contains("start"))         ans.Add("[" + para.IndexOf("start") +"] Rather minify use 'start'");
            if (para.Contains("begin"))         ans.Add("[" + para.IndexOf("begin") +"] Rather minify use 'begin' ");
            if (para.Contains("just"))          ans.Add("[" + para.IndexOf("just") + "] Rather minify use 'just' ");
            // https://qz.com/647121/five-weak-words-you-should-avoid-and-what-to-use-instead/
            if (para.Contains("things")) ans.Add("[" + para.IndexOf("things") + "] Rather minify use 'things'. e.g. 'this article said a lot of things' with 'this article discussed the principles of...'");
            if (para.Contains("stuff")) ans.Add("[" + para.IndexOf("stuff") + "] Rather minify use 'stuff'. e.g. 'this article said a lot of things' with 'this article discussed the principles of...'");
            if (para.Contains("i believe")) ans.Add("[" + para.IndexOf("i believe") + "] Rather minify use 'I believe'. Doesn't make reader comfident. e.g. 'I believe the reasercher as a great point here' vs. 'the researcher has a great point here'");
            if (para.Contains("i feel")) ans.Add("[" + para.IndexOf("i feel") + "] Rather minify use 'i feel'. believe e.g. 'I believe the reasercher as a great point here' vs. 'the researcher has a great point here'");
            if (para.Contains("i think")) ans.Add("[" + para.IndexOf("i think") + "] Rather minify use 'i think'. believe e.g. 'I believe the reasercher as a great point here' vs. 'the researcher has a great point here' ");
            if (para.Contains("what")) ans.Add("[" + para.IndexOf("what") + "] Rather minify use 'what'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            if (para.Contains("is")) ans.Add("[" + para.IndexOf("is") + "] Rather minify use 'is'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            if (para.Contains("are")) ans.Add("[" + para.IndexOf("are") + "] Rather minify use 'are'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            if (para.Contains("am")) ans.Add("[" + para.IndexOf("am") + "] Rather minify use 'am'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            if (para.Contains("very")) ans.Add("[" + para.IndexOf("very") + "] Rather minify use 'very'. e.g. 'scientists are very excited with...' with 'scientists are excited with...' is better");

            // if (para.Contains("")) ans.Add("[" + para.IndexOf("") + "] Rather minify use '' ");

            var speller = RunLongSpellCheck(para);
            if (speller != null)
            {
                ans.Add("Spelling? " + speller);
            }

            // Tenses
            var spacedpara = para.Replace('.', ' ')
                        .Replace('"', ' ')
                        .Replace(',', ' ')
                        .Replace(';', ' ');
            foreach (var k in tenseDict.Keys)
            {
                if (spacedpara.Contains(k))
                {
                    ans.Add("Tense Check: " + k);
                }
            }

            // Debug
            //ans.Add("This isnt a problem. Just a test.");
            // =========================================================
            // = End Checks                                            =
            // =========================================================
            return ans;
        }

        private enum DictionaryDirection {
            WhatNext,
            GoUp,
            GoDown
        }

        /// <summary>
        /// Spellcheck entire novel
        /// </summary>
        /// <param name="ling">Line to check</param>
        internal static string RunLongSpellCheck(string ling)
        {
            string spellErrors = "";
            var words = ling.Split(' ');
            for (int i = 0; i < words.Count(); i++)
            {
                var truncated = words[i].Replace('.', ' ')
                        .Replace('"', ' ')
                        .Replace(',', ' ')
                        .Replace(';', ' ')
                        .Replace(" ", "");
                if (spellChecker.Check(truncated).Count() > 0)
                {
                    if (truncated.ToUpper() != "I")
                    {
                        spellErrors += (i + 1).ToString() + ":" + truncated + ", ";
                    }
                }
            }
            if (spellErrors.Length > 0)
            {
                return spellErrors;
            }
            return null;
        }
    }
}
