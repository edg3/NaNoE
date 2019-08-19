﻿using System;
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
        private static Dictionary<string, string> veryDict = new Dictionary<string, string>();

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

            var verys = File.OpenRead("verys.txt");
            using (StreamReader verysReader = new StreamReader(verys))
            {
                string line = null;
                while ((line = verysReader.ReadLine()) != null)
                {
                    var verbSplit = line.Split(',');
                    veryDict.Add(verbSplit[0], verbSplit[1]);
                }
            }
            verys.Close();
        }

        /// <summary>
        /// A simple way to precess the paragraphs you make for a novel.
        /// </summary>
        /// <param name="para">Paragraph to check</param>
        /// <returns>List of issues, empty list = no problems</returns>
        public static List<string> Process(string para)
        {
            var ans = new List<string>();
            para = para.Replace(",", " ")
                       .Replace(".", " ")
                       .Replace(";", " ")
                       .Replace("\"", " ");
            para += "  ";
            var lowpara = "  " + para.ToLower();

            // =========================================================
            // = Checks added                                          =
            // =========================================================

            // Spelling
            var splt = para.Split(' ');
            for (int i = 0; i < splt.Length; i++)
            {
                var whichUsed = splt[i];
                while (whichUsed.EndsWith(".") || whichUsed.EndsWith(",") || whichUsed.EndsWith(";") || whichUsed.EndsWith(":") || whichUsed.EndsWith(" "))
                {
                    whichUsed = whichUsed.Remove(whichUsed.Length - 1);
                }
            }

            // Side note: this is definitely going to find things that shouldn't be in this - like portions of other words. "is" is in "sister" for example
            // [ replace 'to be' and 'to have' ]
            if (lowpara.Contains(" to be ")
             || lowpara.Contains(" to have ")) ans.Add("[" + para.IndexOf("to ") +"]replace 'to be' and 'to have' with something");
            // [ ly = 'He quickly ran across the park.' fixed to 'He darted across the park.' ]
            if (lowpara.Contains("ly "))           ans.Add("[" + para.IndexOf("ly ") + "]replace '-ly' more descriptive: e.g. not 'her eyes were deadly', rather 'with an evil glare she looked at me'");
            /// [ ing = 'I turned and Mary was glaring at me.' fixed to 'I turned and Mary glared.' ]
            if (lowpara.Contains("ing "))          ans.Add("[" + para.IndexOf("ing ") + "]replace '-ing' words with minimal words. e.g. 'she is running daily now' with 'she runs every morning now'");
            // [ -> Begin, begins, began, beginning, start, starts, started, starting = 'he started to run' fixed by 'he ran' ]
            if (lowpara.Contains(" begin "))         ans.Add("[" + para.IndexOf("begin ") +"] Rather don't use 'begin', simplify it");
            if (lowpara.Contains(" began "))         ans.Add("[" + para.IndexOf("began ") +"] Rather don't use 'began', simplify it");
            if (lowpara.Contains(" start "))         ans.Add("[" + para.IndexOf("start ") +"] Rather don't use 'start', simplify it");
            // [ when, then, suddenly, immediately, always, often, already, finally = 'I immediately ran through the door.' fixed to 'I ran through the door.' ]
            if (lowpara.Contains(" when "))          ans.Add("[" + para.IndexOf("when ") + "] Rather minify use 'when', make literal, we dont need this leading word");
            if (lowpara.Contains(" then "))          ans.Add("[" + para.IndexOf("then ") + "] Rather minify use 'then', make literal, we dont need this leading word");
            if (lowpara.Contains(" suddenly "))      ans.Add("[" + para.IndexOf("suddenly ") + "] Rather minify use 'suddenly', make literal, we dont need this leading word");
            if (lowpara.Contains(" immediately "))   ans.Add("[" + para.IndexOf("immediately ") + "] Rather minify use 'immediately', make literal, we dont need this leading word");
            if (lowpara.Contains(" always "))        ans.Add("[" + para.IndexOf("always ") + "] Rather minify use 'always', make literal, we dont need this leading word");
            if (lowpara.Contains(" often "))         ans.Add("[" + para.IndexOf("often ") +"] Rather minify use 'often', make literal, we dont need this leading word");
            if (lowpara.Contains(" already "))       ans.Add("[" + para.IndexOf("already ") + "] Rather minify use 'already', make literal, we dont need this leading word");
            if (lowpara.Contains(" finally "))       ans.Add("[" + para.IndexOf("finally ") + "] Rather minify use 'finally', make literal, we dont need this leading word");
            // [ See, Look, Hear, Know, Realize, Wonder, Decided, Notice, Feel, Remember, Think, That, Of, Really, Very, Down, Up, Then, Start, begin, Just = reword ]
            if (lowpara.Contains(" see "))           ans.Add("[" + para.IndexOf("see ") +"] Rather minify use 'see' ");
            if (lowpara.Contains(" look "))          ans.Add("[" + para.IndexOf("look ") +"] Rather minify use 'look' ");
            if (lowpara.Contains(" hear "))          ans.Add("[" + para.IndexOf("hear ") +"] Rather minify use 'hear' ");
            if (lowpara.Contains(" know "))          ans.Add("[" + para.IndexOf("know ") +"] Rather minify use 'know' ");
            if (lowpara.Contains(" realize "))       ans.Add("[" + para.IndexOf("realize ") +"] Rather minify use 'realize' ");
            if (lowpara.Contains(" realise "))       ans.Add("[" + para.IndexOf("realise ") +"] Rather minify use 'realise' ");
            if (lowpara.Contains(" wonder "))        ans.Add("[" + para.IndexOf("wonder ") +"] Rather minify use 'wonder' ");
            if (lowpara.Contains(" decided "))       ans.Add("[" + para.IndexOf("decided ") +"] Rather minify use 'decided' ");
            if (lowpara.Contains(" notice "))        ans.Add("[" + para.IndexOf("notice ") +"] Rather minify use 'notice' ");
            if (lowpara.Contains(" feel "))          ans.Add("[" + para.IndexOf("feel ") +"] Rather minify use 'feel' ");
            if (lowpara.Contains(" remember "))      ans.Add("[" + para.IndexOf("remember ") +"] Rather minify use 'remember' ");
            if (lowpara.Contains(" think "))         ans.Add("[" + para.IndexOf("think ") +"] Rather minify use 'think' ");
            if (lowpara.Contains(" that "))          ans.Add("[" + para.IndexOf("that ") +"] Rather minify use 'that' ");
            if (lowpara.Contains(" of "))            ans.Add("[" + para.IndexOf("of ") +"] Rather minify use 'of' ");
            if (lowpara.Contains(" really "))        ans.Add("[" + para.IndexOf("really ") +"] Rather minify use 'really', make literal, e.g. 'the swimmer really performed admirably' with 'the swimmer performed admirably'. Slacker descriptive");
            // if (lowpara.Contains(" very "))          ans.Add("[" + para.IndexOf("very ") +"] Rather minify use 'very'H");
            if (lowpara.Contains(" down "))          ans.Add("[" + para.IndexOf("down ") +"] Rather minify use 'down'");
            if (lowpara.Contains(" up "))            ans.Add("[" + para.IndexOf("up ") +"] Rather minify use 'up'");
            if (lowpara.Contains(" then "))          ans.Add("[" + para.IndexOf("then ") +"] Rather minify use 'then'");
            if (lowpara.Contains(" start "))         ans.Add("[" + para.IndexOf("start ") +"] Rather minify use 'start'");
            if (lowpara.Contains(" begin "))         ans.Add("[" + para.IndexOf("begin ") +"] Rather minify use 'begin' ");
            if (lowpara.Contains(" just "))          ans.Add("[" + para.IndexOf("just ") + "] Rather minify use 'just'. Permission word. e.g. 'I just get bugged by apples' vs 'apples unfortunately bug me'");
            // https://qz.com/647121/five-weak-words-you-should-avoid-and-what-to-use-instead/
            if (lowpara.Contains(" things ")) ans.Add("[" + para.IndexOf("things ") + "] Rather minify use 'things'. e.g. 'this article said a lot of things' with 'this article discussed the principles of...'");
            if (lowpara.Contains(" stuff ")) ans.Add("[" + para.IndexOf("stuff ") + "] Rather minify use 'stuff'. e.g. 'this article said a lot of things' with 'this article discussed the principles of...'");
            if (lowpara.Contains(" i believe ")) ans.Add("[" + para.IndexOf("I believe ") + "] Rather minify use 'I believe'. Doesn't make reader comfident. e.g. 'I believe the reasercher as a great point here' vs. 'the researcher has a great point here'");
            if (lowpara.Contains(" i feel ")) ans.Add("[" + para.IndexOf("I feel ") + "] Rather minify use 'i feel'. believe e.g. 'I believe the reasercher as a great point here' vs. 'the researcher has a great point here'");
            if (lowpara.Contains(" i think ")) ans.Add("[" + para.IndexOf("I think ") + "] Rather minify use 'i think'. believe e.g. 'I believe the reasercher as a great point here' vs. 'the researcher has a great point here' ");
            if (lowpara.Contains(" what ")) ans.Add("[" + para.IndexOf("what ") + "] Rather minify use 'what'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            if (lowpara.Contains(" is ")) ans.Add("[" + para.IndexOf("is ") + "] Rather minify use 'is'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            if (lowpara.Contains(" are ")) ans.Add("[" + para.IndexOf("are ") + "] Rather minify use 'are'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            if (lowpara.Contains(" am ")) ans.Add("[" + para.IndexOf("am ") + "] Rather minify use 'am'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            if (lowpara.Contains(" very ")) ans.Add("[" + para.IndexOf("very ") + "] Rather minify use 'very'. e.g. 'scientists are very excited with...' with 'scientists are excited with...' is better");
            // https://www.skillsyouneed.com/write/cliches-to-avoid.html - portion
            if (lowpara.Contains(" in a nutshell ")) ans.Add("[" + para.IndexOf("in a nutshell ") + "] This is cliched 'in a nutshell'");
            if (lowpara.Contains(" at long last ")) ans.Add("[" + para.IndexOf("at long last ") + "] This is cliched 'at long last'");
            if (lowpara.Contains(" going forward ")) ans.Add("[" + para.IndexOf("going forward ") + "] This is cliched 'going forward'");
            if (lowpara.Contains(" all walks of life ")) ans.Add("[" + para.IndexOf("all walks of life ") + "] This is cliched 'all walks of life'");
            if (lowpara.Contains(" at the end of the day ")) ans.Add("[" + para.IndexOf("at the end of the day ") + "] This is cliched 'at the end of the day'");
            if (lowpara.Contains(" bring the table ")) ans.Add("[" + para.IndexOf("bring the table ") + "] This is cliched 'bring the table'");
            if (lowpara.Contains(" as bold as brass ")) ans.Add("[" + para.IndexOf("as bold as brass ") + "] This is cliched 'as bold as brass'");
            if (lowpara.Contains(" uphill battle ")) ans.Add("[" + para.IndexOf("uphill battle ") + "] This is cliched 'uphill battle'");
            if (lowpara.Contains(" if it ain't broke  don't fix it ")) ans.Add("[" + para.IndexOf("if it ain't broke  don't fix it ") + "] This is cliched 'if it ain't broke, don't fix it'");
            if (lowpara.Contains(" too little  too late ")) ans.Add("[" + para.IndexOf("too little  too late ") + "] This is cliched 'too little, too late'");
            if (lowpara.Contains(" sleeping like the dead ")) ans.Add("[" + para.IndexOf("sleeping like the dead ") + "] This is cliched 'sleeping like the dead'");
            if (lowpara.Contains(" actions speak louder than words ")) ans.Add("[" + para.IndexOf("actions speak louder than words ") + "] This is cliched 'actions speak louder than words'");
            if (lowpara.Contains(" two wrongs don't make a right ")) ans.Add("[" + para.IndexOf("two wrongs don't make a right ") + "] This is cliched 'two wrongs don't make a right'");
            if (lowpara.Contains(" never say never ")) ans.Add("[" + para.IndexOf("never say never ") + "] This is cliched 'never say never'");
            if (lowpara.Contains(" laughter is the best medicine ")) ans.Add("[" + para.IndexOf("laughter is the best medicine ") + "] This is cliched 'laughter is the best medicine'");
            // misc - these can be missed unfortunately
            if (lowpara.Contains(" one of ")) ans.Add("[" + para.IndexOf("one of ") + "] Rather minify use 'one of'. Be more specific 'one of the sciences' vs 'the scientist' is better");
            if (lowpara.Contains(" some ")) ans.Add("[" + para.IndexOf("some ") + "] Rather minify use 'some' words (sometimes, someone, etc). It is vague and detracts from the story");
            if (lowpara.Contains(" thing ")) ans.Add("[" + para.IndexOf("thing ") + "] Rather minify use 'thing'. What thing? Tell us.");
            if (lowpara.Contains("  therefore ")) ans.Add("[" + para.IndexOf("therefore ") + "] Rather minify use 'therefore', sentences shouldnt start with it");
            if (lowpara.Contains("  however ")) ans.Add("[" + para.IndexOf("however ") + "] Rather minify use 'however', sentences shouldnt start with it");
            if (lowpara.Contains("  because ")) ans.Add("[" + para.IndexOf("because ") + "] Rather minify use 'because', sentences shouldn't start with it");
            if (lowpara.Contains("  and ")) ans.Add("[" + para.IndexOf("and ") + "] Rather minify use 'and', sentences shouldn't start with it");
            if (lowpara.Contains("  but ")) ans.Add("[" + para.IndexOf("but ") + "] Rather minify use 'but', sentences shouldn't start with it");
            // Very Accurate = exact
            if (para.Contains(" very "))
            {
                ans.Add("[ Minimised use of 'very' ]");
                ans.Add(" - first: " + para.IndexOf(" very "));
                foreach (var k in veryDict.Keys)
                {
                    if (para.Contains("very " + k)) ans.Add(" - very " + k + " => " + veryDict[k] + " @ " + para.IndexOf("very " + k));
                }
            }


            // if (para.Contains("")) ans.Add("[" + para.IndexOf("") + "] Rather minify use '' ");

            var speller = RunLongSpellCheck(para);
            if (speller != null)
            {
                ans.Add("Spelling? " + speller);
            }

            // Tenses
            foreach (var k in tenseDict.Keys)
            {
                if (para.Contains(k))
                {
                    ans.Add("Tense Check: " + k + ": ~" + para.IndexOf(k));
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
                if (truncated != "")
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
