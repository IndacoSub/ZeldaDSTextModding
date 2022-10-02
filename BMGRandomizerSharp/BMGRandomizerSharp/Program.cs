using System;
using System.IO;
using System.Text.RegularExpressions;

namespace BMGRandomizerSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0 || args.Length > 2)
            {
                Console.WriteLine("Invalid argument(s)! Expected a folder as the first argument, and [optionally] a second folder.");
                Console.WriteLine("The folder[s] should only contain the .bmg_out files to randomize.");
                return;
            }
            string cross_language_folder = "";
            if(args.Length > 1)
            {
                cross_language_folder = args[1];
            }
            string folder = "";
            if(args.Length > 0)
            {
                folder = args[0];
            }
            Start(folder, cross_language_folder);
        }

        public static void Start(string folder, string second_folder)
        {
            bool search_second_folder = second_folder.Length > 0;
            Console.WriteLine("Randomizing content inside folder: " + folder);
            bool CanBreak = false;
            int FileNumber = 0;
            List<string> FilesPath = new List<string>();
            List<List<string>> FilesContent = new List<List<string>>();
            List<List<string>> SecondFolderFilesContent = new List<List<string>>();
            int file_index = 0;
            ulong total_lines = 0;
            string[] files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
            foreach (string fit in files)
            {
                if (Directory.Exists(fit))
                {
                    continue;
                }
                string path = fit;
                FilesPath.Add(path);
                FilesContent.Add(new List<string>());

                string temp = "";
                using (StreamReader sr = new StreamReader(fit, System.Text.Encoding.Unicode, true))
                {
                    while (!sr.EndOfStream)
                    {
                        temp = sr.ReadLine() ?? "";
                        FilesContent[file_index].Add(temp.Trim());
                        total_lines++;
                    }
                }
                file_index++;
            }
            FileNumber = file_index;
            file_index = 0;
            total_lines = 0;

            string[] second_folder_files = search_second_folder ? Directory.GetFiles(second_folder, "*.*", SearchOption.AllDirectories) : new string[0];
            foreach (string fit in second_folder_files)
            {
                if (Directory.Exists(fit))
                {
                    continue;
                }
                string path = fit;
                SecondFolderFilesContent.Add(new List<string>());

                string temp = "";
                using (StreamReader sr = new StreamReader(fit, true))
                {
                    while (!sr.EndOfStream)
                    {
                        temp = sr.ReadLine() ?? "";
                        SecondFolderFilesContent[file_index].Add(temp.Trim());
                        total_lines++;
                    }
                }
                file_index++;
            }

            if(FilesContent.Count != SecondFolderFilesContent.Count && search_second_folder)
            {
                Console.WriteLine("Different number of files inside the two folders!");
                return;
            }

            if(search_second_folder && SecondFolderFilesContent.Count > 0)
            {
                Console.WriteLine("Managed to read second folder!");
            }

            bool stop_all = false;
            for (int i = 0; i < FilesContent.Count; i++) // List of files
            {
                if (stop_all && CanBreak)
                {
                    break;
                }

                for (int j = 0; j < FilesContent[i].Count; j++) // Lines in a single file
                {
                    if (stop_all && CanBreak)
                    {
                        break;
                    }
                    string current_line = FilesContent[i][j];
                    if(current_line.Length <= 0)
                    {
                        continue;
                    }
                    bool current_contains_escape = ContainsEscapeSequences(current_line);
                    bool maintain_current_line = false;
                    if (current_contains_escape)
                    {
                        maintain_current_line = !CanSkipEscapeAllSequences(current_line);
                    }
                    if (maintain_current_line)
                    {
                        continue;
                    }

                    if(search_second_folder && SecondFolderFilesContent.Count > 0)
                    {
                        if (j >= SecondFolderFilesContent[i].Count)
                        {
                            continue;
                        }
                        string corresponding_line = SecondFolderFilesContent[i][j];
                        if (corresponding_line.Length <= 0)
                        {
                            continue;
                        }

                        bool corresponding_contains_escape = ContainsEscapeSequences(corresponding_line);
                        bool maintain_corresponding_line = false;
                        if (corresponding_contains_escape)
                        {
                            maintain_corresponding_line = !CanSkipEscapeAllSequences(corresponding_line);
                        }
                        if (maintain_corresponding_line)
                        {
                            continue;
                        }
                    }

                    bool do_actually_randomize_instead_of_just_swapping = true;
                    int swap_line_file_index = i;
                    int swap_line_number = j;
                    if (do_actually_randomize_instead_of_just_swapping)
                    {
                        string swap_line = "";
                        List<string> swap_line_file = new List<string>();
                        bool maintain_swap_line = false;
                        ulong tries_maintain_swap = 0;
                        do
                        {
                            if (stop_all && CanBreak)
                            {
                                break;
                            }
                            ulong tries_same = 0;
                            do
                            {
                                if (stop_all && CanBreak)
                                {
                                    break;
                                }
                                swap_line_file_index = GetRandomNumber(0, FileNumber - 1);
                                swap_line_file = search_second_folder && SecondFolderFilesContent.Count > 0 ? SecondFolderFilesContent[swap_line_file_index] : FilesContent[swap_line_file_index];
                                swap_line_number = GetRandomNumber(0, swap_line_file.Count - 1);
                                tries_same++;
                                if (tries_same > 1_000_000)
                                {
                                    stop_all = true;
                                }
                            } while (swap_line_file_index == i && swap_line_number == j); // same as this one
                            tries_maintain_swap++;
                            if (tries_maintain_swap > 1_000_000)
                            {
                                stop_all = true;
                            }
                            if (swap_line_file.Count <= swap_line_number)
                            {
                                continue;
                            }
                            swap_line = swap_line_file[swap_line_number];
                            if (swap_line.Length <= 0)
                            {
                                continue;
                            }
                            bool swap_contains_escape = ContainsEscapeSequences(swap_line);
                            if (swap_contains_escape)
                            {
                                maintain_swap_line = !CanSkipEscapeAllSequences(swap_line);
                            }
                        } while (maintain_swap_line || swap_line.Length <= 1);
                    }

                    bool swap_back = false;
                    if(search_second_folder && SecondFolderFilesContent.Count > 0)
                    {
                        if (swap_back)
                        {
                            (FilesContent[i][j], SecondFolderFilesContent[swap_line_file_index][swap_line_number]) = (SecondFolderFilesContent[swap_line_file_index][swap_line_number], FilesContent[i][j]);
                        } else
                        {
                            FilesContent[i][j] = SecondFolderFilesContent[swap_line_file_index][swap_line_number];
                        }
                    } else
                    {
                        if(swap_back)
                        {
                            (FilesContent[i][j], FilesContent[swap_line_file_index][swap_line_number]) = (FilesContent[swap_line_file_index][swap_line_number], FilesContent[i][j]);
                        } else
                        {
                            FilesContent[i][j] = FilesContent[swap_line_file_index][swap_line_number];
                        }
                    }
                }
            }

            for (int k = 0; k < FilesPath.Count; k++)
            {
                string fpath = FilesPath[k];
                File.Delete(fpath);
                List<string> current_file = FilesContent[k];
                using (StreamWriter sw = new StreamWriter(fpath, true, System.Text.Encoding.Unicode))
                {
                    foreach (string lines in current_file)
                    {
                        string outstr = lines;
                        sw.WriteLine(outstr);
                    }
                }
            }

            Console.WriteLine("Done!");
            Console.WriteLine("Remember to copy the randomized files to the \"edit\" folder needed by ReimportBMGFile.py!");
        }

        public static int GetRandomNumber(int min, int max)
        {
            Random rnd = new Random();
            int ret = rnd.Next(min, max + 1);
            return ret;
        }

        public static int countFreq(string pat, string txt)
        {
            return Regex.Matches(pat, Regex.Escape(txt)).Count;
        }

        public static bool ContainsEscapeSequences(string full_line)
        {

            return full_line.Contains("[") && full_line.Contains("]");
        }

        public static bool ShouldSkipEscapeSequence(string escape_sequence, string full)
        {
            List<string> NecessaryESs = new List<string> {
            "[0:0000]", // Option 1 (ST)
            "[0:0100]", // Option 2 (ST)
            "[0:0200]", // Option 3 (ST)
            "[0:0f001e00]", // Exiting from shops

            "[1:0000]", // Option 1 (PH)
            "[1:0100]", // Option 2 (PH)
            "[1:0200]", // Option 3 (PH)

            "[1:1a000100]", // Northwestern Sea chart
            "[1:1a000200]", // Southeastern Sea chart
            "[1:1a000300]", // Northeastern Sea chart
            "[1:1a000000]", // Southwestern Sea chart?

            "[1:17000000]", // Show current map (Southwestern Sea chart)
            "[3:fe000700]", // ...Hide current map?

            "[0:0f00af00]", // Cutscene text?
            };

            bool ret =
                escape_sequence != NecessaryESs[0] && escape_sequence != NecessaryESs[1] && escape_sequence != NecessaryESs[2]
                &&
                escape_sequence != NecessaryESs[3] && escape_sequence != NecessaryESs[4] && escape_sequence != NecessaryESs[5]
                &&
                escape_sequence != NecessaryESs[6] && escape_sequence != NecessaryESs[7] && escape_sequence != NecessaryESs[8]
                &&
                escape_sequence != NecessaryESs[9] && escape_sequence != NecessaryESs[10] && escape_sequence != NecessaryESs[11]
                &&
                escape_sequence != NecessaryESs[12] && escape_sequence != NecessaryESs[13]
                ;

            return ret;
        }

        public static bool CanSkipEscapeAllSequences(string current_line)
        {
            if(current_line.Normalize().Trim().Length <= 1)
            {
                return false;
            }
            int escapes_number = countFreq(current_line, "]");
            if(escapes_number == 0)
            {
                return true;
            }

            bool can_skip_all_of_them = true;
            string temp = current_line;
            do
            {
                if(!can_skip_all_of_them)
                {
                    break;
                }
                int bracket_index = temp.IndexOf("[");
                temp = temp.Substring(bracket_index);
                int bracket_end = temp.IndexOf("]");
                string temp2 = temp;
                temp2 = temp2.Substring(0, bracket_end + 1);
                bool skippable_escape = ShouldSkipEscapeSequence(temp2, current_line);
                can_skip_all_of_them &= skippable_escape;
                temp = temp.Substring(bracket_end + 1);
            } while (temp.Contains("[") || temp.Contains("]"));

            return can_skip_all_of_them;
        }
    }
}