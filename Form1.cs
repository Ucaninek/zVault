using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Transitions;

namespace zVault
{
    public partial class Form1 : Form
    {
        int? midHeightDefault;
        string midTextDefault = null;
        string[] files = null;
        bool linkedOpen = false;
        bool disableLinkedDrop = true;
        bool disableDragDrop = false;
        CancellationTokenSource cancellationSource = new CancellationTokenSource();
        private CancellationToken cancellation;

        public Form1()
        {
            InitializeComponent();
            cancellation = cancellationSource.Token;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            midTextDefault = MidTxt.Text;
            midHeightDefault = MidTxt.Height;
            Panel.Enabled = false;

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                if (File.Exists(args[1]))
                {
                    linkedOpen = true;
                    disableLinkedDrop = linkedOpen;
                    this.TopMost = true;
                    ProcessFiles(new string[] { args[1] });
                }
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (disableLinkedDrop && linkedOpen) return;
            if (disableDragDrop) return;
            string[] _files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (File.Exists(_files[0])) {
                e.Effect = DragDropEffects.Copy;
                Transition.run(this, "BackColor", Color.White, new TransitionType_EaseInEaseOut(150));
                Transition.run(MidTxt, "Text", "drop it here", new TransitionType_EaseInEaseOut(150));
            }
            else
            {
                Transition.run(MidTxt, "Text", "that wont work :(", new TransitionType_EaseInEaseOut(150));
            }

        }

        private void FastAlert(string s)
        {
            MidTxt.ForeColor = SystemColors.ButtonShadow;
            Transition.run(MidTxt, "Text", s, new TransitionType_EaseInEaseOut(1));
            Transition.run(MidTxt, "ForeColor", Color.Maroon, new TransitionType_Flash(2, 500));
        }

        private void Form1_DragLeave(object sender, EventArgs e)
        {
            if (disableDragDrop) return;
            Transition.run(this, "BackColor", SystemColors.Control, new TransitionType_EaseInEaseOut(150));
            Transition.run(MidTxt, "Text", midTextDefault, new TransitionType_EaseInEaseOut(250));
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            disableDragDrop = true;
            ProcessFiles(files);
        }

        private void ProcessFiles(string[] files)
        {
            bool? tmpIsLocked = null;
            bool differentLockedStatuses = false;
            foreach (string path in files)
            {
                if (tmpIsLocked != null)
                {
                    if (path.EndsWith(".zv") != tmpIsLocked) differentLockedStatuses = true;
                }
                else
                    tmpIsLocked = path.EndsWith(".zv");
                if (!File.Exists(path))
                {
                    MessageBox.Show("Folders are not lockable");
                    Environment.Exit(1);
                }
            }

            if (differentLockedStatuses)
            {
                MessageBox.Show("Cannot process locked and unlocked files at the same time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Transition.run(Title, "Text", (files.Length > 1 ? "Lock Files" : "Lock a File"), new TransitionType_EaseInEaseOut(250));

            if (files[0].EndsWith(".zv") || files[0].EndsWith(".zVault"))
            {
                Transition.run(MidTxt, "Text", String.Format("Enter the password for {0}", files.Length > 1 ? files.Length + " files" : Path.GetFileName(files[0].Replace(".zv", ""))), new TransitionType_EaseInEaseOut(150));
            }
            else
            {
                Transition.run(MidTxt, "Text", String.Format("Create a password for {0}", files.Length > 1 ? files.Length + " files" : Path.GetFileName(files[0])), new TransitionType_EaseInEaseOut(150));
            }

            Transition t = new Transition(new TransitionType_EaseInEaseOut(250));
            t.add(MidTxt, "Height", MidTxt.Height - 40);
            t.run();

            MidTxt.Dock = DockStyle.Top;
            Panel.Show();
            Panel.Enabled = true;
            this.files = files;
            PassBox.Focus();
        }

        private void BtnProceed_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(PassBox.Text)) return;
            BtnProceed.Enabled = false;
            PassBox.Enabled = false;
            cancellationSource = new CancellationTokenSource(); // Reset the cancellation source
            cancellation = cancellationSource.Token;
            Task.Run(async () => await Crypto(PassBox.Text, files)).ContinueWith(a =>
            {
                if (!a.IsFaulted) disableDragDrop = false;
            });
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            PassBox.Enabled = true;
            PassBox.Focus();
            disableDragDrop = false;

            if (linkedOpen) Environment.Exit(0);
            Title.Text = "Welcome Back";
            MidTxt.Text = midTextDefault;
            PassBox.Clear();

            cancellationSource.Cancel(); //cancel after text change so detailed text from Crypto can be shown

            MidTxt.Dock = DockStyle.Fill;
            Panel.Hide();
            Panel.Enabled = false;
            PassBox.Enabled = true;
            PassBox.Focus();

            BtnProceed.Enabled = true;
        }

        private async Task Crypto(string password, string[] files)
        {
            int completed = 0;
            bool fileEncrypted = files[0].EndsWith(".zv") || files[0].EndsWith(".zVault");

            foreach (string file in files)
            {
                try
                {
                    fileEncrypted = file.EndsWith(".zv") || file.EndsWith(".zVault");
                    this.Invoke((MethodInvoker)delegate
                    {
                        Transition.run(Title, "Text", (fileEncrypted ? "Unlocking" : "Locking"), new TransitionType_EaseInEaseOut(250));
                    });
                    this.Invoke((MethodInvoker)delegate { MidTxt.Text = String.Format("Reading file {1} of {2}", Path.GetFileName(file), completed + 1, files.Length); });
                    this.Invoke((MethodInvoker)delegate { MidTxt.Text = String.Format("({1} of {2}) Processing {0}", Path.GetFileName(file), completed + 1, files.Length); });
                    if (fileEncrypted) //going to be decrypted
                    {
                        await zVault.Crypto.DecryptFileAsync(file, password, cancellation, new Progress<int>(progress =>
                        {
                            // Update the progress UI
                            this.Invoke((MethodInvoker)delegate
                            {
                                MidTxt.Text = string.Format("Decrypting {0} - ({3}%)", Path.GetFileName(file), completed + 1, files.Length, progress);
                            });
                        }));

                    }
                    else
                    {
                        // Encrypt the file using IProgress
                        await zVault.Crypto.EncryptFileAsync(file, password, cancellation, new Progress<int>(progress =>
                        {
                            // Update the progress UI
                            this.Invoke((MethodInvoker)delegate
                            {
                                MidTxt.Text = string.Format("Encrypting {0} - ({3}%)", Path.GetFileName(file), completed + 1, files.Length, progress);
                            });
                        }));
                    }

                    if (fileEncrypted) File.Move(file, file.Replace(".zv", ""));
                    else File.Move(file, file + ".zv");

                    this.Invoke((MethodInvoker)delegate
                    {
                        Transition.run(Title, "Text", (fileEncrypted ? "Unlocked!" : "Locked!"), new TransitionType_EaseInEaseOut(250));
                        PassBox.Text = "";
                        MidTxt.Text = "Cool, waiting for another one..";
                        MidTxt.Dock = DockStyle.Fill;
                        Panel.Hide();
                        Panel.Enabled = false;
                        BtnProceed.Enabled = true;
                        disableLinkedDrop = false;
                    });

                    if (cancellation.IsCancellationRequested) return;

                    completed++;

                    PassBox.Invoke((MethodInvoker)delegate
                    {
                        PassBox.Enabled = true;
                        PassBox.Focus();
                    });
                }
                catch (Exception ex)
                {
                    if (ex is TaskCanceledException)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            Title.Text = "Cancelled";
                            MidTxt.Text = String.Format("Completed {0} of {1}, waiting for another one..", completed, files.Length);
                            MidTxt.Dock = DockStyle.Fill;
                            Panel.Hide();
                            Panel.Enabled = false;
                            BtnProceed.Enabled = true;
                        });
                    }
                    else
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            MidTxt.Text = String.Format("Enter the password for {0}", files.Length > 1 ? files.Length + " files" : Path.GetFileName(files[0].Replace(".zv", "")));
                            BtnProceed.Enabled = true;
                            PassBox.Enabled = true;
                            PassBox.Focus();
                        });

                        string error = ex.Message;
                        if (ex is CryptographicException)
                        {
                            if (ex.Message.ToLower().Contains("not a complete block"))
                                error = "Supplied file is corrupt";
                            else if (ex.Message.ToLower().Contains("padding is invalid"))
                                error = "Incorrect password";
                        }
                        else if (ex is IOException)
                        {
                            error = "IO error occurred";
                        }
                        FastAlert(error);
                        this.Invoke((MethodInvoker)delegate
                        {
                            Transition.run(Title, "Text", "Error Occurred", new TransitionType_EaseInEaseOut(250));
                        });
                    }
                }
            }
        }
    }
}
