using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Remwave.Client
{
    public partial class SettingsWindow : Form
    {
        public bool ApplyChanges = false;
        public bool AbortClosing = false;
        private ClientSettings mClientSettings;
        private ClientSettingsSerializer mClientSettingsSerializer;
        private void BrandComponent()
        {
            this.Icon = Properties.Resources.desktop;
        }
        private void LocalizeComponent()
        {
            tabPageGeneral.Text = Properties.Localization.txtSettingsGeneralTab;// "General";
            groupBoxBasicSettings.Text = Properties.Localization.txtSettingsBasicSettings;//"Basic settings";
            checkBoxEnableQA.Text = Properties.Localization.txtSettingsEnableQAReporting;//"Enable Quality Agent reporting";
            checkBoxConfirmClosing.Text = Properties.Localization.txtConfirmBeforeClosing;//"Confirm before closing";
            checkBoxStartup.Text = Properties.Localization.txtSettingsLaunchWhenWindowsStarts;//"Launch when Windows starts";
            tabPagePhone.Text = Properties.Localization.txtSettingsPhoneTab;//"Phone";
            groupBoxRingTone.Text = Properties.Localization.txtSettingsPhoneRingTone;//"Ring Tone";
            buttonRingToneSelect.Text = Properties.Localization.txtSettingsPhoneRingToneSelect;//"Select";
            radioButtonRingToneCustom.Text = Properties.Localization.txtSettingsPhoneRingToneCustom;//"Custom";
            radioButtonRingToneDefault.Text = Properties.Localization.txtSettingsPhoneRingToneDefault;//"Default";
            groupBoxAudioCodecs.Text = Properties.Localization.txtSettingsPhoneAudioCodecs;//"Audio Codecs";
            labelEnabledCodecs.Text = Properties.Localization.txtSettingsPhoneEnabledCodecs;//"Enabled codecs:";
            labelAvailableCodecs.Text = Properties.Localization.txtSettingsPhoneAvailableCodecs;//"Available codecs:";
            groupBoxAudioVolume.Text = Properties.Localization.txtSettingsPhoneAudioVolume;//"Audio Volume";
            labelSpeaker.Text = Properties.Localization.txtSettingsPhoneSpeakerVolume;//"Speaker";
            labelMicrophone.Text = Properties.Localization.txtSettingsPhoneMicrophoneVolume;//"Microphone";
            buttonCancel.Text = Properties.Localization.txtSettingsCancelButton;//"Cancel";
            buttonOK.Text = Properties.Localization.txtSettingsOKButton;//"OK";
            buttonApply.Text = Properties.Localization.txtSettingsApplyButton;//"Apply";
            checkBoxEnableSipDiagnosticLogging.Text = Properties.Localization.txtSettingsPhoneEnableSIPLogging;//"Enable SIP diagnostic logging";

            checkBoxContactsLocal.Text = Properties.Localization.txtSettingsContactsLocalStore;//Use local computer storage
            checkBoxContactsOutlook.Text = Properties.Localization.txtSettingsContactsOutlookStore;//Use Outlook contacts
            checkBoxContactsServer.Text = Properties.Localization.txtSettingsContactsServerStore;//Use Remote Server storage            
            groupBoxContactsStores.Text = Properties.Localization.txtSettingsContactsStores; //Contacts stores

            groupBoxLanguage.Text = Properties.Localization.txtSettingsLanguage;//Language
        }


        public SettingsWindow()
        {
            InitializeComponent();
        }
        public SettingsWindow(String path)
        {
            InitializeComponent();

            LocalizeComponent();
            BrandComponent();

            //TO DO: Open configuration
            mClientSettingsSerializer = new ClientSettingsSerializer(path);
            mClientSettings = mClientSettingsSerializer.Load();



            //Basic
            checkBoxConfirmClosing.Checked = mClientSettings.ProgramConfirmClosing;
            checkBoxEnableQA.Checked = mClientSettings.ProgramEnableQualityAgent;
            checkBoxStartup.Checked = mClientSettings.ProgramAtStartup;

            checkBoxContactsServer.Checked = mClientSettings.ContactsEnableServerStore;
            checkBoxContactsOutlook.Checked = mClientSettings.ContactsEnableOutlookStore;
            checkBoxContactsLocal.Checked = true; //Always available

            //Language
            String cultureInfo = Thread.CurrentThread.CurrentUICulture.Name;

            if (cultureInfo == "es-ES")
            {
                radioButtonEspanol.Checked = true;
            }
            else if (cultureInfo == "de-DE")
            {
                radioButtonDeutsch.Checked = true;
            }
            else if (cultureInfo == "it-CH")
            {
                radioButtonItalian.Checked = true;
            }
            else //    if (cultureInfo == "en-US")
            {
                radioButtonEnglish.Checked = true;
            }

                




            //Phone
            checkBoxEnableSipDiagnosticLogging.Checked = mClientSettings.PhoneEnableSIPDiagnostic;
            trackBarMicrophone.Value = mClientSettings.PhoneAudioMicrophoneVolume;
            trackBarSpeaker.Value = mClientSettings.PhoneAudioSpeakerVolume;

            listBoxAvailableCodecs.Items.Clear();
            listBoxEnabledCodecs.Items.Clear();

            foreach (AudioCodec audioCodec in mClientSettings.PhoneAvailableMediaFormats)
            {
                if (!listBoxAvailableCodecs.Items.Contains(audioCodec)) listBoxAvailableCodecs.Items.Add(audioCodec);
            }

            foreach (AudioCodec audioCodec in mClientSettings.PhoneEnabledMediaFormats)
            {
                if (!listBoxEnabledCodecs.Items.Contains(audioCodec)) listBoxEnabledCodecs.Items.Add(audioCodec);
            }

            radioButtonRingToneCustom.Checked = !mClientSettings.PhoneDefaultRingToneEnabled;
            radioButtonRingToneDefault.Checked = mClientSettings.PhoneDefaultRingToneEnabled;


            if (File.Exists(mClientSettings.PhoneCustomRingTone))
            {
                FileInfo fileInfo = new FileInfo(mClientSettings.PhoneCustomRingTone);
                if (fileInfo.Extension.ToLower() == ".mp3")
                {
                    textBoxRingTonePath.Text = mClientSettings.PhoneCustomRingTone;
                }
            }

        }


        

        private void ApplySettings()
        {
            this.ApplyChanges = true;
            //TO DO : Save configuration
            mClientSettings.PhoneEnableSIPDiagnostic = checkBoxEnableSipDiagnosticLogging.Checked;
            mClientSettings.PhoneAudioMicrophoneVolume = trackBarMicrophone.Value;
            mClientSettings.PhoneAudioSpeakerVolume = trackBarSpeaker.Value;
            mClientSettings.PhoneEnabledMediaFormats.Clear();
            foreach (AudioCodec codec in listBoxEnabledCodecs.Items)
            {
                mClientSettings.PhoneEnabledMediaFormats.Add(codec);
            }

            mClientSettings.PhoneCustomRingTone = "";
            mClientSettings.PhoneDefaultRingToneEnabled = radioButtonRingToneDefault.Checked;

            if (File.Exists(textBoxRingTonePath.Text))
            {
                FileInfo fileInfo = new FileInfo(textBoxRingTonePath.Text);
                if (fileInfo.Extension.ToLower() == ".mp3")
                {
                    mClientSettings.PhoneCustomRingTone = textBoxRingTonePath.Text;
                }
            }



            mClientSettings.ProgramAtStartup = checkBoxStartup.Checked;
            mClientSettings.ProgramConfirmClosing = checkBoxConfirmClosing.Checked;
            mClientSettings.ProgramEnableQualityAgent = checkBoxEnableQA.Checked;

            if (radioButtonEspanol.Checked)
            {
                mClientSettings.ProgramLanguage = "es-ES";
            }
            else if (radioButtonDeutsch.Checked)
            {
                mClientSettings.ProgramLanguage = "de-DE";
            }
            else if (radioButtonItalian.Checked)
            {
                mClientSettings.ProgramLanguage = "it-CH";
            }
            else
            {
                mClientSettings.ProgramLanguage = "en-US";
            }

            mClientSettings.ContactsEnableOutlookStore = checkBoxContactsOutlook.Checked;
            mClientSettings.ContactsEnableServerStore = checkBoxContactsServer.Checked;

            mClientSettingsSerializer.Save(mClientSettings);
        }

        private void buttonAddCodec_Click(object sender, EventArgs e)
        {
            if (listBoxAvailableCodecs.SelectedItem != null)
            {
                if (listBoxEnabledCodecs.Items.Count >= 4)
                {
                    listBoxEnabledCodecs.Items.RemoveAt(listBoxEnabledCodecs.Items.Count - 1);
                }
                if (listBoxEnabledCodecs.Items.Contains(listBoxAvailableCodecs.SelectedItem))
                {
                    listBoxEnabledCodecs.Items.Remove(listBoxAvailableCodecs.SelectedItem);
                }
                listBoxEnabledCodecs.Items.Add(listBoxAvailableCodecs.SelectedItem);
            }
        }

        private void buttonRemoveCodec_Click(object sender, EventArgs e)
        {
            if (listBoxEnabledCodecs.SelectedItem != null)
            {
                listBoxEnabledCodecs.Items.Remove(listBoxEnabledCodecs.SelectedItem);
            }
            else if (listBoxEnabledCodecs.Items.Count > 0)
            {
                listBoxEnabledCodecs.Items.RemoveAt(listBoxEnabledCodecs.Items.Count - 1);
            }
        }

        private void radioButtonRingToneDefault_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRingToneCustom.Checked = !radioButtonRingToneDefault.Checked;
        }

        private void radioButtonRingToneCustom_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRingToneDefault.Checked = !radioButtonRingToneCustom.Checked;
        }

        private void buttonRingToneSelect_Click(object sender, EventArgs e)
        {
            if ((DialogResult.OK == openFileDialogRingTone.ShowDialog() && openFileDialogRingTone.FileName != null))
            {
                textBoxRingTonePath.Text = openFileDialogRingTone.FileName;
                radioButtonRingToneCustom.Checked = true;
            }
        }

        private void radioButtonEnglish_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonEnglish.Checked)
            {
                radioButtonDeutsch.Checked=false;
                radioButtonEspanol.Checked = false;
                radioButtonItalian.Checked = false;
            }
        }

        private void radioButtonDeutsch_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDeutsch.Checked)
            {
                radioButtonEnglish.Checked = false;
                radioButtonEspanol.Checked = false;
                radioButtonItalian.Checked = false;
            }
        }

        private void radioButtonEspanol_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonEspanol.Checked)
            {
                radioButtonDeutsch.Checked = false;
                radioButtonEnglish.Checked = false;
                radioButtonItalian.Checked = false;
            }
        }
        private void radioButtonItalian_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonEspanol.Checked)
            {
                radioButtonDeutsch.Checked = false;
                radioButtonEnglish.Checked = false;
                radioButtonEspanol.Checked = false;
            }
        }
        private void buttonApply_Click(object sender, EventArgs e)
        {
            ApplySettings();
            this.AbortClosing = true;
            this.Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.AbortClosing = false;
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ApplySettings();
            this.AbortClosing = false;
            this.Close();
        }

        
    }
}