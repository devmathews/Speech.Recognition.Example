using System;
using Syn.Speech.Api;
using System.IO;
using Syn.Logging;

namespace Speech.Recognition.Example
{
	class MainClass
	{
		static Configuration speechConfiguration;
		static StreamSpeechRecognizer speechRecognizer;

		static void LogReceived (object sender, LogReceivedEventArgs e)
		{
			Console.WriteLine (e.Message);
		}

		public static void Main (string[] args)
		{
			Logger.LogReceived += LogReceived;

			var modelsDirectory = Path.Combine (Directory.GetCurrentDirectory (), "Models");
			var audioDirectory = Path.Combine (Directory.GetCurrentDirectory (), "Audio");
			var audioFile = Path.Combine (audioDirectory, "Long Audio 2.wav");

			if (!Directory.Exists (modelsDirectory)||!Directory.Exists(audioDirectory)) {
				Console.WriteLine ("No Models or Audio directory found!! Aborting...");
				Console.ReadLine ();
				return;
			}

			speechConfiguration = new Configuration ();
			speechConfiguration.AcousticModelPath=modelsDirectory;
			speechConfiguration.DictionaryPath = Path.Combine (modelsDirectory, "cmudict-en-us.dict");
			speechConfiguration.LanguageModelPath = Path.Combine (modelsDirectory, "en-us.lm.dmp");

			speechConfiguration.UseGrammar = true;
			speechConfiguration.GrammarPath = modelsDirectory;
			speechConfiguration.GrammarName = "hello";


			speechRecognizer = new StreamSpeechRecognizer (speechConfiguration);
			var stream = new FileStream (audioFile, FileMode.Open);
			speechRecognizer.StartRecognition (stream);

			Console.WriteLine ("Transcribing...");
			var result = speechRecognizer.GetResult ();

			if (result != null) {
				Console.WriteLine ("Result: " + result.GetHypothesis ());
			} else {
				Console.WriteLine ("Sorry! Coudn't Transcribe");
			}

			Console.ReadLine ();
		}
	}
}
