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
            // [ replace 'to be' and 'to have' ]
            if ((searchString = GetLocations(" " + para + " ", " to be ")) != "")
                ans.Add("[~" + searchString + "] replace 'to be' with something");
            if ((searchString = GetLocations(" " + para + " ", " to have ")) != "")
                ans.Add("[~" + searchString + "] replace 'to have' with something");
            // [ ly = 'He quickly ran across the park.' fixed to 'He darted across the park.' ]
            if ((searchString = GetLocations(" " + para + " ", "ly ")) != "")
                ans.Add("[" + searchString + "] replace '-ly' more descriptive: e.g. not 'her eyes were deadly', rather 'with an evil glare she looked at me'");
            /// [ ing = 'I turned and Mary was glaring at me.' fixed to 'I turned and Mary glared.' ]
            if ((searchString = GetLocations(" " + para + " ", "ing ")) != "")
                ans.Add("[" + searchString + "] replace '-ing' words with minimal words. e.g. 'she is running daily now' with 'she runs every morning now'");
            // [ -> Begin, begins, began, beginning, start, starts, started, starting = 'he started to run' fixed by 'he ran' ]
            if ((searchString = GetLocations(" " + para + " ", "begin ")) != "")
                ans.Add("[" + searchString +"] Rather don't use 'begin', simplify it");
            if ((searchString = GetLocations(" " + para + " ", "began ")) != "")
                ans.Add("[" + searchString +"] Rather don't use 'began', simplify it");
            if ((searchString = GetLocations(" " + para + " ", "start ")) != "")
                ans.Add("[" + searchString +"] Rather don't use 'start', simplify it");
            // [ when, then, suddenly, immediately, always, often, already, finally = 'I immediately ran through the door.' fixed to 'I ran through the door.' ]
            if ((searchString = GetLocations(" " + para + " ", " when ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'when', make literal, we dont need this leading word");
            if ((searchString = GetLocations(" " + para + " ", " then ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'then', make literal, we dont need this leading word");
            if ((searchString = GetLocations(" " + para + " ", " suddenly ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'suddenly', make literal, we dont need this leading word");
            if ((searchString = GetLocations(" " + para + " ", " immediately ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'immediately', make literal, we dont need this leading word");
            if ((searchString = GetLocations(" " + para + " ", " always ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'always', make literal, we dont need this leading word");
            if ((searchString = GetLocations(" " + para + " ", " often ")) != "")
                ans.Add("[" + searchString +"] Rather minify use 'often', make literal, we dont need this leading word");
            if ((searchString = GetLocations(" " + para + " ", " already ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'already', make literal, we dont need this leading word");
            if ((searchString = GetLocations(" " + para + " ", " finally ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'finally', make literal, we dont need this leading word");
            // [ See, Look, Hear, Know, Realize, Wonder, Decided, Notice, Feel, Remember, Think, That, Of, Really, Very, Down, Up, Then, Start, begin, Just = reword ]
            if ((searchString = GetLocations(" " + para + " ", " see ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'see' (eg. observe, look at, behold, descry, examine, inspect, regard, espy, perceive, comprehend, discern, look on, be present, escort, attend, meet a bet, cover a bet, speak to, speak with, ...)");
            if ((searchString = GetLocations(" " + para + " ", " look ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'look' (eg. appearance, aspect, looks, expression, gaze, stare, scrutiny, inspection, contemplation, visual search, reconnaissance,glance, quick cast of the eyes, survey, squint, glimpse, peek, peep, ...)");
            if ((searchString = GetLocations(" " + para + " ", " hear ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'hear' (eg. listen to, hearken, hark, attend to, make out, become aware of, catch, descry, apprehend, take in, detect, perceive by the ear, overhear, eavesdrop, be advised, find out, learn, have it on good authority,preside over, put on trial, ...)");
            if ((searchString = GetLocations(" " + para + " ", " know ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'know' (eg. be aware of, be cognizant of, be acquainted with, be informed, be in possession of the facts, have knowledge of, comprehend, apprehend, grasp, see into,perceive, discern, distinguish, identify, ...)");
            if ((searchString = GetLocations(" " + para + " ", " realize ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'realize' (eg. accomplish, actualize, effectuate, make good, recognize, apprehend, discern, clear, make a profit from, obtain, ...");
            if ((searchString = GetLocations(" " + para + " ", " wonder ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'wonder' (eg. surprise, awe, stupefaction, admiration, wonderment, astonishment, wondering, miracle, curiosity, oddity, rarity, freak, phenomenon, ...)");
            if ((searchString = GetLocations(" " + para + " ", " decided ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'decided' (eg. settled, decided upon, arranged for, emphatic, determined, clear, clear, clear-cut, categorical, decisive, determined, firm, ...)");
            if ((searchString = GetLocations(" " + para + " ", " notice ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'notice' (eg, note, notification, intimation , comments, remark, enlightenment, mark, remark, discern, look at, mention, comment on, notify, ...)");
            if ((searchString = GetLocations(" " + para + " ", " feel ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'feel' (eg. touch, handle, finger, explore, stroke, palm, caress, sense, perceive, apprehend, be aware of, consider, hold,appear, exhibit, fumble, grabble, grope, ...)");
            if ((searchString = GetLocations(" " + para + " ", "remember")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'remember' (eg. recollect, recognize, summon up, relive, think of, bring to mind, refresh one's memory, be reminded of, revive, keep in mind, retain, memorize, know by heart, learn,bethink, mind, recall, ...)");
            if ((searchString = GetLocations(" " + para + " ", " think ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'think' (eg. cogitate, reason, deliberate, ideate, muse, ponder, consider, contemplate, deliberate, be convinced, deem, imagine, guess, conceive, invent, recollect, ...)");
            if ((searchString = GetLocations(" " + para + " ", " that ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'that' (eg. the, this, one, a certain, a well known, a particular, such, even so, all things considered, not so very, not so, rather less, ...)");
            if ((searchString = GetLocations(" " + para + " ", " of ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'of' (replaced by eg. from, out from, away from, proceed from, about, as concerns, peculiar to, attributed to, characterized by, as regards, in regard to, in reference to, appropriate to, like, belongs to, related to, relation to, native to, consequent to, based on, akin to, connected with, ...)");
            if ((searchString = GetLocations(" " + para + " ", " really ")) != "")
                ans.Add("[" + searchString +"] Rather minify use 'really', make literal, e.g. 'the swimmer really performed admirably' with 'the swimmer performed admirably'. Slacker descriptive");
            if ((searchString = GetLocations(" " + para + " ", " down ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'down' (eg. forward, headlong, downward, downhill, downstairs, below, depressed, underneath, inferior, lowly, below par, downcast, depressed, inoperative, out of order, ...)");
            if ((searchString = GetLocations(" " + para + " ", " up ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'up' (eg. at the top of, at the crest of, at the summit of, upward, uphill, skyward, heavenward, lapsed, elapsed, run out, under consideration, being scrutinized, after, in order, ...)");
            if ((searchString = GetLocations(" " + para + " ", " then ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'then' (eg. at that time, before, years ago, at that point, all at once, soon after, before long, next, later, thereupon, but at the same time, on the other hand, ...)");
            if ((searchString = GetLocations(" " + para + " ", " start ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'start' (eg. inception, commencement, inauguration, source, derivation, spring, commence, rise, inaugurate, start off, rouse, incite, light, set on fire, ...)");
            if ((searchString = GetLocations(" " + para + " ", " begin ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'begin' (eg. start, cause, initiate, inaugurate, commence, occasion, impel, produce, effect, set in motion, commence, get under way, start, start out, set out, set in, come out, approach, commence, ...)");
            if ((searchString = GetLocations(" " + para + " ", " just ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'just'. Permission word. e.g. 'I just get bugged by apples' vs 'apples unfortunately bug me'");
            // https://qz.com/647121/five-weak-words-you-should-avoid-and-what-to-use-instead/
            if ((searchString = GetLocations(" " + para + " ", " things ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'things'. e.g. 'this article said a lot of things' with 'this article discussed the principles of...'");
            if ((searchString = GetLocations(" " + para + " ", " stuff ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'stuff'. e.g. 'this article said a lot of things' with 'this article discussed the principles of...'");
            if ((searchString = GetLocations(" " + para + " ", " i believe ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'I believe'. Doesn't make reader comfident. e.g. 'I believe the reasercher as a great point here' vs. 'the researcher has a great point here'");
            if ((searchString = GetLocations(" " + para + " ", " i feel ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'i feel'. believe e.g. 'I believe the reasercher as a great point here' vs. 'the researcher has a great point here'");
            if ((searchString = GetLocations(" " + para + " ", " i think ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'i think'. believe e.g. 'I believe the reasercher as a great point here' vs. 'the researcher has a great point here' ");
            if ((searchString = GetLocations(" " + para + " ", " what ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'what'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            if ((searchString = GetLocations(" " + para + " ", " is ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'is'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            if ((searchString = GetLocations(" " + para + " ", " are ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'are'. Write in a more active like 'sally mailed the letters' instead of 'the letter are mailed by sally'");
            if ((searchString = GetLocations(" " + para + " ", " am ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'am'. Write in a more active like 'sally mailed the letter' instead of 'the letter was mailed by sally'");
            // if (lowpara.Contains(" very ")) ans.Add("[" + para.IndexOf("very ") + "] Rather minify use 'very'. e.g. 'scientists are very excited with...' with 'scientists are excited with...' is better");
            // https://www.skillsyouneed.com/write/cliches-to-avoid.html - portion
            if ((searchString = GetLocations(" " + para + " ", " in a nutshell ")) != "")
                ans.Add("[" + searchString + "] This is cliched 'in a nutshell'");
            if ((searchString = GetLocations(" " + para + " ", " at long last ")) != "")
                ans.Add("[" + searchString + "] This is cliched 'at long last'");
            if ((searchString = GetLocations(" " + para + " ", " going forward ")) != "")
                ans.Add("[" + searchString + "] This is cliched 'going forward'");
            if ((searchString = GetLocations(" " + para + " ", " all walks of life ")) != "")
                ans.Add("[" + searchString + "] This is cliched 'all walks of life'");
            if ((searchString = GetLocations(" " + para + " ", " at the end of the day ")) != "")
                ans.Add("[" + searchString + "] This is cliched 'at the end of the day'");
            if ((searchString = GetLocations(" " + para + " ", " bring the table")) != "")
                ans.Add("[" + searchString + "] This is cliched 'bring the table'");
            if ((searchString = GetLocations(" " + para + " ", " as bold as brass ")) != "")
                ans.Add("[" + searchString + "] This is cliched 'as bold as brass'");
            if ((searchString = GetLocations(" " + para + " ", " uphill battle ")) != "")
                ans.Add("[" + searchString + "] This is cliched 'uphill battle'");
            if((searchString = GetLocations(" " + para + " ", " if it ain't broke don't fix it ")) != "")
                ans.Add("[" + searchString + "] This is cliched 'if it ain't broke, don't fix it'");
            if ((searchString = GetLocations(" " + para + " ", " too little too late")) != "")
                ans.Add("[" + searchString + "] This is cliched 'too little, too late'");
            if ((searchString = GetLocations(" " + para + " ", " sleeping like the dead ")) != "")
                ans.Add("[" + searchString + "] This is cliched 'sleeping like the dead'");
            if ((searchString = GetLocations(" " + para + " ", " actions speak louder than words ")) != "")
                ans.Add("[" + searchString + "] This is cliched 'actions speak louder than words'");
            if ((searchString = GetLocations(" " + para + " ", " two wrongs don't make a right ")) != "")
                ans.Add("[" + searchString + "] This is cliched 'two wrongs don't make a right'");
            if ((searchString = GetLocations(" " + para + " ", " never say never ")) != "")
                ans.Add("[" + searchString + "] This is cliched 'never say never'");
            if ((searchString = GetLocations(" " + para + " ", " laughter is the best medicine ")) != "")
                ans.Add("[" + searchString+ "] This is cliched 'laughter is the best medicine'");
            // misc - these can be missed unfortunately
            if ((searchString = GetLocations(" " + para + " ", " one of ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'one of'. Be more specific 'one of the sciences' vs 'the scientist' is better");
            if ((searchString = GetLocations(" " + para + " ", "  some")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'some' words (sometimes, someone, etc). It is vague and detracts from the story");
            if ((searchString = GetLocations(" " + para + " ", "  therefore ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'therefore', sentences shouldnt start with it");
            if ((searchString = GetLocations(" " + para + " ", "  however ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'however', sentences shouldnt start with it");
            if ((searchString = GetLocations(" " + para + " ", "  because")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'because', sentences shouldn't start with it");
            if ((searchString = GetLocations(" " + para + " ", "  and ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'and', sentences shouldn't start with it");
            if ((searchString = GetLocations(" " + para + " ", "  but ")) != "")
                ans.Add("[" + searchString + "] Rather minify use 'but', sentences shouldn't start with it");
            // Very Accurate = exact
            // if ((searchString = GetLocations(" " + para + " ", " very ")) != "") ans.Add("[ Minimised use of 'very' ] [" + searchString + "]");
            
            // https://dianaurban.com/words-you-should-cut-from-your-writing-immediately
            if ((searchString = GetLocations(" " + para + " ", " really ")) != "") ans.Add("[" + searchString + "] Words we should delete: really - unwanted modifier, 'he ran very quickly along...' vs. 'he sprinted along...'");
            if ((searchString = GetLocations(" " + para + " ", " very ")) != "") ans.Add(  "[" + searchString + "] Words we should delete: very - unwanted modifier, 'he ran very quickly along...' vs. 'he sprinted along...'");
            if ((searchString = GetLocations(" " + para + " ", " that ")) != "") ans.Add(  "[" + searchString + "] Words we should delete: that - not needed, 'this is the very important blog post that I read' vs. 'this is the important blog post I read'.");
            if ((searchString = GetLocations(" " + para + " ", " just ")) != "") ans.Add(  "[" + searchString + "] Words we should delete: just - not needed, try take it out, its very repetitive use usually");
            if ((searchString = GetLocations(" " + para + " ", " then ")) != "") ans.Add(  "[" + searchString + "] Words we should delete: then - not needed, '... the sidewalk. then bob pointed and laughed. ...' vs. '... the sidewalk. bob pointed and laughed. ...'");
            if ((searchString = GetLocations(" " + para + " ", " start ")) != "")    ans.Add("[" + searchString + "] Words we should delete: start - most of the time not needed, only in interuptions usually...?");
            if ((searchString = GetLocations(" " + para + " ", " begin ")) != "")    ans.Add("[" + searchString + "] Words we should delete: begin - most of the time not needed, only in interuptions usually...?");
            if ((searchString = GetLocations(" " + para + " ", " began ")) != "")    ans.Add("[" + searchString + "] Words we should delete: began - most of the time not needed, only in interuptions usually...?");
            if ((searchString = GetLocations(" " + para + " ", " begun ")) != "")    ans.Add("[" + searchString + "] Words we should delete: begun - most of the time not needed, only in interuptions usually...?");
            if ((searchString = GetLocations(" " + para + " ", " rather ")) != "")   ans.Add("[" + searchString + "] Words we should delete: rather - it isn't 'rather dull' its 'dull'");
            if ((searchString = GetLocations(" " + para + " ", " quite ")) != "")    ans.Add("[" + searchString + "] Words we should delete: quite - it isn't 'rather dull' its 'dull'");
            if ((searchString = GetLocations(" " + para + " ", " somewhat ")) != "") ans.Add("[" + searchString + "] Words we should delete: somewhat - it isn't 'rather dull' its 'dull'");
            if ((searchString = GetLocations(" " + para + " ", " somehow ")) != "")  ans.Add("[" + searchString + "] Words we should delete: somehow - it isn't 'rather dull' its 'dull'");
            if ((searchString = GetLocations(" " + para + " ", " said ")) != "") ans.Add("[" + searchString + "] Words we should adjust: dialogue tags - rather remove them, we dont need to tell the reader they are talking due to \" tages? - perhaps opt for surrounding dialogue with actions like '... last week\". John slumped onto his chair. \"I didn't... - the note here is allow the reader to rely on tracking it themselves"); ;
            if ((searchString = GetLocations(" " + para + " ", " replied ")) != "") ans.Add("[" + searchString + "] Words we should adjust: dialogue tags - rather remove them, we dont need to tell the reader they are talking due to \" tages? - perhaps opt for surrounding dialogue with actions like '... last week\". John slumped onto his chair. \"I didn't... - the note here is allow the reader to rely on tracking it themselves"); ;
            if ((searchString = GetLocations(" " + para + " ", " asked ")) != "") ans.Add("[" + searchString + "] Words we should adjust: dialogue tags - rather remove them, we dont need to tell the reader they are talking due to \" tages? - perhaps opt for surrounding dialogue with actions like '... last week\". John slumped onto his chair. \"I didn't... - the note here is allow the reader to rely on tracking it themselves"); ;
            if ((searchString = GetLocations(" " + para + " ", " down ")) != "")        ans.Add("[" + searchString + "] Words we should delete: down - I sat down on the floor could be I sat on the floor");
            if ((searchString = GetLocations(" " + para + " ", " up ")) != "")          ans.Add("[" + searchString + "] Words we should delete: up - I stood up could be I stood");
            if ((searchString = GetLocations(" " + para + " ", " wonder ")) != "")      ans.Add("[" + searchString + "] Words we should delete: wonder - we have the reader listening to thoughts, why wonder? rather let the reader know");
            if ((searchString = GetLocations(" " + para + " ", " ponder ")) != "")      ans.Add("[" + searchString + "] Words we should delete: ponder - we have the reader listening to thoughts, why wonder? rather let the reader know");
            if ((searchString = GetLocations(" " + para + " ", " think ")) != "")       ans.Add("[" + searchString + "] Words we should delete: think - we have the reader listening to thoughts, why wonder? rather let the reader know");
            if ((searchString = GetLocations(" " + para + " ", " though ")) != "")      ans.Add("[" + searchString + "] Words we should delete: though - we have the reader listening to thoughts, why wonder? rather let the reader know");
            if ((searchString = GetLocations(" " + para + " ", " feel ")) != "")        ans.Add("[" + searchString + "] Words we should delete: feel - we have the reader listening to thoughts, why wonder? rather let the reader know");
            if ((searchString = GetLocations(" " + para + " ", " felt ")) != "")        ans.Add("[" + searchString + "] Words we should delete: felt - we have the reader listening to thoughts, why wonder? rather let the reader know");
            if ((searchString = GetLocations(" " + para + " ", " understand ")) != "")  ans.Add("[" + searchString + "] Words we should delete: understand - we have the reader listening to thoughts, why wonder? rather let the reader know");
            if ((searchString = GetLocations(" " + para + " ", " realize ")) != "")     ans.Add("[" + searchString + "] Words we should delete: realize - we have the reader listening to thoughts, why wonder? rather let the reader know");
            if ((searchString = GetLocations(" " + para + " ", " breath ")) != "")      ans.Add("[" + searchString + "] Words we should delete: breath - look at emotional thesaurus to try replace this");
            if ((searchString = GetLocations(" " + para + " ", " breathe ")) != "")     ans.Add("[" + searchString + "] Words we should delete: breath - look at emotional thesaurus to try replace this");
            if ((searchString = GetLocations(" " + para + " ", " inhale ")) != "")      ans.Add("[" + searchString + "] Words we should delete: inhale - look at emotional thesaurus to try replace this");
            if ((searchString = GetLocations(" " + para + " ", " exhale ")) != "")      ans.Add("[" + searchString + "] Words we should delete: exhale - look at emotional thesaurus to try replace this");
            if ((searchString = GetLocations(" " + para + " ", " shrug ")) != "")       ans.Add("[" + searchString + "] Words we should delete: shrug - authors use this too frequently, minimise it");
            if ((searchString = GetLocations(" " + para + " ", " nod ")) != "")         ans.Add("[" + searchString + "] Words we should delete: nod - authors use this too frequently, minimise it");
            if ((searchString = GetLocations(" " + para + " ", " reach ")) != "")       ans.Add("[" + searchString + "] Words we should delete: reach - authors use this too frequently, minimise it");


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
                        ans.Add("Bad word: [before " + (i + 1).ToString() + "] " + s);
                    }
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
    }
}
