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
        private static Dictionary<string, string> veryDict = new Dictionary<string, string>();
        private static List<string> badDict = new List<string>();
        private static string[] ignoredLy = new string[] { "fly", "ally", "sly", "ply ", "rely", "family" };
        private static string[] ignoredIng = new string[] { "ting", "ring", "sing ", "ding", "king ", "ping", "wing", "morning" };
        private static string suggestedDialogue = "[suggested]\n" +
                                                  " Anger: Shouted, bellowed, yelled, snapped, cautioned, rebuked.\n\n" +
                                                  "Affection: Consoled, comforted, reassured, admired, soothed.\n\n" +
                                                  "Excitement: Shouted, yelled, babbled, gushed, exclaimed.\n\n" +
                                                  "Fear: Whispered, stuttered, stammered, gasped, urged, hissed, babbled, blurted.\n\n" +
                                                  "Determination: Declared, insisted, maintained, commanded.\n\n" +
                                                  "Happiness: Sighed, murmured, gushed, laughed.\n\n" +
                                                  "Sadness: Cried, mumbled, sobbed, sighed, lamented.\n\n" +
                                                  "Conflict: Jabbed, sneered, rebuked, hissed, scolded, demanded, threatened, insinuated, spat, glowered.\n\n" +
                                                  "Making up: Apologised, relented, agreed, reassured, placated, assented.\n\n" +
                                                  "Amusement: Teased, joked, laughed, chuckled, chortled, sniggered, tittered, guffawed, giggled, roared.\n\n" + 
                                                  "Storytelling: Related, recounted, continued, emphasized, remembered, recalled, resumed, concluded.";

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

            var bads = File.OpenRead("bad");
            using (StreamReader badReader = new StreamReader(bads))
            {
                string line = null;
                while ((line = badReader.ReadLine()) != null)
                {
                    badDict.Add(line);
                }
            }
            bads.Close();
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

            string searchString = "";

            // Side note: this is definitely going to find things that shouldn't be in this - like portions of other words. "is" is in "sister" for example

            // [ ly = 'He quickly ran across the park.' fixed to 'He darted across the park.' ]
            if ((searchString = GetLocations(" " + para + " ", "ly ", ignoredLy)) != "")
                ans.Add("[" + searchString + "] replace '-ly' more descriptive: e.g. not 'her eyes were deadly', rather 'with an evil glare she looked at me'");
            /// [ ing = 'I turned and Mary was glaring at me.' fixed to 'I turned and Mary glared.' ]
            if ((searchString = GetLocations(" " + para + " ", "ing ", ignoredIng)) != "")
                ans.Add("[" + searchString + "] replace '-ing' words with minimal words. e.g. 'she is running daily now' with 'she runs every morning now'");

            RunCheck(ans, para, "actions speak louder than words", "This is cliched 'actions speak louder than words'");
            RunCheck(ans, para, "all walks of life", "This is cliched 'all walks of life'");
            RunCheck(ans, para, "always", "Rather minify use 'always', make literal, we dont need this leading word");
            RunCheck(ans, para, "am", "Rather minify use 'am'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            RunCheck(ans, para, "and", "Rather minify use 'and', sentences shouldn't start with it");
            RunCheck(ans, para, "are", "Rather minify use 'are'. Write in a more active like 'sally mailed the letters' instead of 'the letter are mailed by sally'");
            RunCheck(ans, para, "as bold as brass", "This is cliched 'as bold as brass'");
            RunCheck(ans, para, "asked", "Words we should adjust: 'asked' dialogue tags - " + suggestedDialogue);
            RunCheck(ans, para, "at long last", "This is cliched 'at long last'");
            RunCheck(ans, para, "at the end of the day", "This is cliched 'at the end of the day'");
            RunCheck(ans, para, "because", "Rather minify use 'because', sentences shouldn't start with it");
            RunCheck(ans, para, "began", "Rather don't use 'began', simplify it");
            RunCheck(ans, para, "began", "Words we should delete: began - most of the time not needed, only in interuptions usually...?");
            RunCheck(ans, para, "begin", "Rather minify use 'begin' (eg. start, cause, initiate, inaugurate, commence, occasion, impel, produce, effect, set in motion, commence, get under way, start, start out, set out, set in, come out, approach, commence, ...");
            RunCheck(ans, para, "begun", "Words we should delete: begun - most of the time not needed, only in interuptions usually...?");
            RunCheck(ans, para, "breath", "Words we should delete: breath - look at emotional thesaurus to try replace this");
            RunCheck(ans, para, "breathe", "Words we should delete: breath - look at emotional thesaurus to try replace this");
            RunCheck(ans, para, "bring the table", "This is cliched 'bring the table'");
            RunCheck(ans, para, "but", "Rather minify use 'but', sentences shouldn't start with it");
            RunCheck(ans, para, "decided", "Rather minify use 'decided' (eg. settled, decided upon, arranged for, emphatic, determined, clear, clear, clear-cut, categorical, decisive, determined, firm, ...");
            RunCheck(ans, para, "down", "Rather minify use 'down' (eg. forward, headlong, downward, downhill, downstairs, below, depressed, underneath, inferior, lowly, below par, downcast, depressed, inoperative, out of order, ...");
            RunCheck(ans, para, "exhale", "Words we should delete: exhale - look at emotional thesaurus to try replace this");
            RunCheck(ans, para, "feel", "Rather minify use 'feel' (eg. touch, handle, finger, explore, stroke, palm, caress, sense, perceive, apprehend, be aware of, consider, hold,appear, exhibit, fumble, grabble, grope, ...");
            RunCheck(ans, para, "felt", "Words we should delete: felt - we have the reader listening to thoughts, why wonder? rather let the reader know");
            RunCheck(ans, para, "going forward", "This is cliched 'going forward");
            RunCheck(ans, para, "hear", "Rather minify use 'hear' (eg. listen to, hearken, hark, attend to, make out, become aware of, catch, descry, apprehend, take in, detect, perceive by the ear, overhear, eavesdrop, be advised, find out, learn, have it on good authority,preside over, put on trial, ...)");
            RunCheck(ans, para, "however", "Rather minify use 'however', sentences shouldnt start with it");
            RunCheck(ans, para, "i believe", "Rather minify use 'I believe'. Doesn't make reader comfident. e.g. 'I believe the reasercher as a great point here' vs. 'the researcher has a great point here");
            RunCheck(ans, para, "i feel", "Rather minify use 'i feel'. believe e.g. 'I believe the reasercher as a great point here' vs. 'the researcher has a great point here");
            RunCheck(ans, para, "i think", "Rather minify use 'i think'. believe e.g. 'I believe the reasercher as a great point here' vs. 'the researcher has a great point here'");
            RunCheck(ans, para, "if it ain't broke, don't fix it", "This is cliched 'if it ain't broke, don't fix it'");
            RunCheck(ans, para, "in a nutshell", "This is cliched 'in a nutshell'");
            RunCheck(ans, para, "inhale", "Words we should delete: inhale - look at emotional thesaurus to try replace this");
            RunCheck(ans, para, "is ", "Rather minify use 'is'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            RunCheck(ans, para, "just", "Words we should delete: just - not needed, try take it out, its very repetitive use usually");
            RunCheck(ans, para, "know", "Rather minify use 'know' (eg. be aware of, be cognizant of, be acquainted with, be informed, be in possession of the facts, have knowledge of, comprehend, apprehend, grasp, see into,perceive, discern, distinguish, identify, ...)");
            RunCheck(ans, para, "laughter is the best medicine", "This is cliched 'laughter is the best medicine'");
            RunCheck(ans, para, "look", "Rather minify use 'look' (eg. appearance, aspect, looks, expression, gaze, stare, scrutiny, inspection, contemplation, visual search, reconnaissance,glance, quick cast of the eyes, survey, squint, glimpse, peek, peep, ...)");
            RunCheck(ans, para, "never say never", "This is cliched 'never say never'");
            RunCheck(ans, para, "nod", "Words we should delete: nod - authors use this too frequently, minimise it");
            RunCheck(ans, para, "notice", "Rather minify use 'notice' (eg, note, notification, intimation , comments, remark, enlightenment, mark, remark, discern, look at, mention, comment on, notify, ...");
            RunCheck(ans, para, "of", "Rather minify use 'of' (replaced by eg. from, out from, away from, proceed from, about, as concerns, peculiar to, attributed to, characterized by, as regards, in regard to, in reference to, appropriate to, like, belongs to, related to, relation to, native to, consequent to, based on, akin to, connected with, ...");
            RunCheck(ans, para, "often", "Rather minify use 'often', make literal, we dont need this leading word");
            RunCheck(ans, para, "one of", "Rather minify use 'one of'. Be more specific 'one of the sciences' vs 'the scientist' is better");
            RunCheck(ans, para, "ponder", "Words we should delete: ponder - we have the reader listening to thoughts, why wonder? rather let the reader know");
            RunCheck(ans, para, "quite", "Words we should delete: quite - it isn't 'quite dull' its 'dull");
            RunCheck(ans, para, "rather", "Words we should delete: rather - it isn't 'rather dull' its 'dull");
            RunCheck(ans, para, "reach", "Words we should delete: reach - authors use this too frequently, minimise it");
            RunCheck(ans, para, "realize", "Words we should delete: realize - we have the reader listening to thoughts, why wonder? rather let the reader know");
            RunCheck(ans, para, "really", "Rather minify use 'of' (replaced by eg. from, out from, away from, proceed from, about, as concerns, peculiar to, attributed to, characterized by, as regards, in regard to, in reference to, appropriate to, like, belongs to, related to, relation to, native to, consequent to, based on, akin to, connected with, ...");
            RunCheck(ans, para, "remember", "Rather minify use 'remember' (eg. recollect, recognize, summon up, relive, think of, bring to mind, refresh one's memory, be reminded of, revive, keep in mind, retain, memorize, know by heart, learn,bethink, mind, recall, ...");
            RunCheck(ans, para, "replied", "Words we should adjust: 'replied' dialogue tags - " + suggestedDialogue);
            RunCheck(ans, para, "said", "Words we should adjust: 'said' dialogue tags - " + suggestedDialogue);
            RunCheck(ans, para, "see", "Rather minify use 'see' (eg. observe, look at, behold, descry, examine, inspect, regard, espy, perceive, comprehend, discern, look on, be present, escort, attend, meet a bet, cover a bet, speak to, speak with, ...)");
            RunCheck(ans, para, "shrug", "Words we should delete: shrug - authors use this too frequently, minimise it");
            RunCheck(ans, para, "sleeping like the dead", "This is cliched 'sleeping like the dead");
            RunCheck(ans, para, "some", "Rather minify use 'some' words (sometimes, someone, etc). It is vague and detracts from the story");
            RunCheck(ans, para, "somehow", "Words we should delete: somehow - it isn't 'somehow dull' its 'dull'");
            RunCheck(ans, para, "somewhat", "Words we should delete: somewhat - it isn't 'somewhat dull' its 'dull'");
            RunCheck(ans, para, "start", "Rather minify use 'start' (eg. inception, commencement, inauguration, source, derivation, spring, commence, rise, inaugurate, start off, rouse, incite, light, set on fire, ...");
            RunCheck(ans, para, "stuff", "Rather minify use 'stuff'. e.g. 'this article said a lot of things' with 'this article discussed the principles of...");
            RunCheck(ans, para, "that", "Rather minify use 'that' (eg. the, this, one, a certain, a well known, a particular, such, even so, all things considered, not so very, not so, rather less, ...");
            RunCheck(ans, para, "then", "Rather minify use 'then' (eg. at that time, before, years ago, at that point, all at once, soon after, before long, next, later, thereupon, but at the same time, on the other hand, ...");
            RunCheck(ans, para, "therefore", "Rather minify use 'therefore', sentences shouldnt start with it");
            RunCheck(ans, para, "things", "Rather minify use 'things'. e.g. 'this article said a lot of things' with 'this article discussed the principles of...");
            RunCheck(ans, para, "think", "Rather minify use 'think' (eg. cogitate, reason, deliberate, ideate, muse, ponder, consider, contemplate, deliberate, be convinced, deem, imagine, guess, conceive, invent, recollect, ...");
            RunCheck(ans, para, "though", "Words we should delete: though - we have the reader listening to thoughts, why wonder? rather let the reader know");
            RunCheck(ans, para, "to be", "replace 'to be' with something");
            RunCheck(ans, para, "to have", "replace 'to have' with something");
            RunCheck(ans, para, "too little too late", "This is cliched 'too little, too late'");
            RunCheck(ans, para, "two wrongs don't make a right", "This is cliched 'two wrongs don't make a right'");
            RunCheck(ans, para, "understand", "Words we should delete: understand - we have the reader listening to thoughts, why wonder? rather let the reader know");
            RunCheck(ans, para, "up", "Rather minify use 'up' (eg. at the top of, at the crest of, at the summit of, upward, uphill, skyward, heavenward, lapsed, elapsed, run out, under consideration, being scrutinized, after, in order, ...");
            RunCheck(ans, para, "uphill battle", "This is cliched 'uphill battle'");
            RunCheck(ans, para, "very", "Rather minify use 'very'. e.g. 'scientists are very excited with...' with 'scientists are excited with...' is better");
            RunCheck(ans, para, "what", "Rather minify use 'what'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            RunCheck(ans, para, "when", "Rather minify use 'when', make literal, we dont need this leading word");
            RunCheck(ans, para, "wonder", "Rather minify use 'wonder' (eg. surprise, awe, stupefaction, admiration, wonderment, astonishment, wondering, miracle, curiosity, oddity, rarity, freak, phenomenon, ...");

            // if (para.Contains("")) ans.Add("[" + para.IndexOf("") + "] Rather minify use '' ");

            var speller = RunLongSpellCheck(para);
            if (speller.Count != 0)
            {
                for (int i = 0; i < speller.Count; i++)
                    ans.Add("[Spelling] " + speller[i] + " [" + FindCharLocation(para, speller[i]) + "]");
            }

            // Tenses
            foreach (var k in tenseDict.Keys)
            {
                if (para.Contains(k))
                {
                    ans.Add("[Tense Check] " + k + " [~" + para.IndexOf(k) + "]");
                }
            }

            CheckBad(ans, para);

            // Debug
            //ans.Add("This isnt a problem. Just a test.");
            // =========================================================
            // = End Checks                                            =
            // =========================================================
            return ans;
        }

        /// <summary>
        /// Pre-emptive attempt at making this better
        /// </summary>
        /// <param name="ans">List of suggested problems</param>
        /// <param name="para">the paragraph</param>
        /// <param name="v1">what to look for</param>
        /// <param name="v2">what to say</param>
        private static void RunCheck(List<string> ans, string para, string v1, string v2)
        {
            if (para.Contains("  "))
            {
                while (para.Contains("  "))
                {
                    para = para.Replace("  ", " ");
                }
            }

            if (para[0] == ' ')
            {
                para.Substring(1);
            }

            var splt = para.Split(' ');
            for (int i = 0; i < splt.Length; ++i)
            {
                if (splt[i] == v1) ans.Add("[word " + (i + 1) + "] " + v2);
            }
        }


        /// <summary>
        /// Look through to see if para contains a bad word
        /// </summary>
        /// <param name="ans">The list of "errors"</param>
        /// <param name="para">The paragraph to check</param>
        private static void CheckBad(List<string> ans, string para)
        {
            var s = "";
            for (int i = 0; i < para.Length; i++)
            {
                if (para[i] != ' ')
                {
                    s += para[i];
                }
                else
                {
                    s = s.Replace(".", "").Replace(",", "").Replace(";", "").Replace("\"", "").Replace("'", "").ToLower();
                    if (badDict.Contains(s))
                    {
                        ans.Add("Potentially bad word: [before " + (i + 1).ToString() + "] " + s);
                    }
                    s = "";
                }
            }

            s = s.Replace(".", "").Replace(",", "").Replace(";", "").Replace("\"", "").Replace("'", "").ToLower();
            if (badDict.Contains(s))
            {
                ans.Add("Bad word: [at end] " + s);
            }
        }

        /// <summary>
        /// Find position of spelling error
        /// </summary>
        /// <param name="para">The paragraph</param>
        /// <param name="v">The spelling error</param>
        /// <returns>String list of positions</returns>
        private static string FindCharLocation(string para, string v)
        {
            var answer = "";

            int i = v.Length;
            for (int x = 0; x < para.Length - i; x++)
            {
                var substring = para.Substring(x, i);
                if (substring == v)
                {
                    answer += (answer.Length > 0 ? ", " : "") + x.ToString();
                }
            }

            return answer;
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
        internal static List<string> RunLongSpellCheck(string ling)
        {
            List<string> errors = new List<string>();
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
                            errors.Add("  [" + ling.IndexOf(truncated) + "]" + truncated);
                        }
                    }
            }
            return errors;
        }

        private static string GetLocations(string line, string what)
        {
            var answer = "";

            line = line.ToLower();

            int i = what.Length;
            for (int x = 0; x < line.Length - i; x++)
            {
                var substring = line.Substring(x, i);
                if ((substring == what) || (substring.ToLower() == what))
                {
                    answer += (answer.Length > 0 ? ", " : "") + (x - 1).ToString();
                }
                else if (substring.Replace(',',' ').Replace('.',' ').Replace(';', ' ').Replace('"', ' ').Replace('\'',' ').ToLower() == what)
                {
                    answer += (answer.Length > 0 ? ", " : "") + (x - 1).ToString();
                }
            }

            return answer;
        }

        private static string GetLocations(string line, string what, string[] ignore)
        {
            var answer = "";

            line = line.ToLower();

            int i = what.Length;
            for (int x = 0; x < line.Length - i; x++)
            {
                var substring = line.Substring(x, i);
                if ((substring == what) || (substring.ToLower() == what))
                {
                    if (!CheckIgnored(line, what, ignore, x)) answer += (answer.Length > 0 ? ", " : "") + (x - 1).ToString();
                }
                else if (substring.Replace(',', ' ').Replace('.', ' ').Replace(';', ' ').Replace('"', ' ').Replace('\'', ' ').ToLower() == what)
                {
                    if (!CheckIgnored(line, what, ignore, x)) answer += (answer.Length > 0 ? ", " : "") + (x - 1).ToString();
                }
            }

            return answer;
        }

        private static bool CheckIgnored(string line, string srchStr, string[] ignore, int x)
        {
            foreach (var ignoreWord in ignore)
            {
                if (x - ignoreWord.Length >= 0)
                {
                    for (int q = x - ignoreWord.Length; q < x + srchStr.Length; q++)
                    {
                        try
                        {   // length troubles?
                            var l = line.Substring(q, ignoreWord.Length);
                            if (ignoreWord == l)
                            {
                                return true;
                            }
                        } catch { }
                    }
                }
            }

            return false;
        }
    }
}
