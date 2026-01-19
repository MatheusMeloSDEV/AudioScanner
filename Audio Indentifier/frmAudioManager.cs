using System;
using System.Windows.Forms;

namespace AudioScanner
{
    public partial class frmAudioManager : Form
    {
        private string _currentAppId = null;
        private bool _isUpdatingUI = false;

        public frmAudioManager()
        {
            InitializeComponent();
            this.Text = "Scanner de Áudio Pro (x64)";

            timerScan.Tick += timerScan_Tick;
            lstAudio.SelectedIndexChanged += LstAudio_SelectedIndexChanged;
            trackVolume.ValueChanged += TrackVolume_ValueChanged;
            chkMute.CheckedChanged += ChkMute_CheckedChanged;

            timerScan.Interval = 1000;
            timerScan.Start();
        }

        private void timerScan_Tick(object sender, EventArgs e)
        {
            if (trackVolume.Focused || chkMute.Focused) return;

            var sessions = VolumeMixer.GetAllSessions();

            if (sessions.Count == lstAudio.Items.Count && !chkAutoLock.Checked) return;

            string savedID = null;
            if (lstAudio.SelectedItem != null) savedID = ((AppItem)lstAudio.SelectedItem).ID;

            lstAudio.Items.Clear();

            AppItem itemToSelect = null;

            foreach (var s in sessions)
            {
                var item = new AppItem
                {
                    Name = s.Name,
                    ID = s.ID,
                    Volume = s.Volume,
                    Muted = s.Muted
                };
                lstAudio.Items.Add(item);

                if (chkAutoLock.Checked && !string.IsNullOrEmpty(txtTarget.Text))
                {
                    if (item.Name.IndexOf(txtTarget.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        item.ID.IndexOf(txtTarget.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        itemToSelect = item;
                    }
                }
                else if (savedID != null && s.ID == savedID)
                {
                    itemToSelect = item;
                }
            }

            if (itemToSelect != null)
            {
                lstAudio.SelectedItem = itemToSelect;
            }
        }

        private void LstAudio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstAudio.SelectedItem == null) return;

            var item = lstAudio.SelectedItem as AppItem;
            _currentAppId = item.ID;
            _isUpdatingUI = true;

            float? vol = VolumeMixer.GetApplicationVolume(_currentAppId);
            bool? mute = VolumeMixer.GetApplicationMute(_currentAppId);

            if (vol.HasValue) trackVolume.Value = (int)vol.Value;
            if (mute.HasValue) chkMute.Checked = mute.Value;

            this.Text = $"Controlando: {item.Name} (ID: {item.ID})";
            gbAudio.Text = $"Controle de Áudio - {item.Name}";
            _isUpdatingUI = false;
        }

        private void TrackVolume_ValueChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUI || string.IsNullOrEmpty(_currentAppId)) return;

            VolumeMixer.SetApplicationVolume(_currentAppId, trackVolume.Value);

            if (lstAudio.SelectedItem is AppItem item)
            {
                item.Volume = trackVolume.Value;
                int idx = lstAudio.SelectedIndex;
                _isUpdatingUI = true;
                lstAudio.Items[idx] = item;
                _isUpdatingUI = false;
            }
        }

        private void ChkMute_CheckedChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUI || string.IsNullOrEmpty(_currentAppId)) return;

            VolumeMixer.SetApplicationMute(_currentAppId, chkMute.Checked);

            if (lstAudio.SelectedItem is AppItem item)
            {
                item.Muted = chkMute.Checked;
                int idx = lstAudio.SelectedIndex;
                _isUpdatingUI = true;
                lstAudio.Items[idx] = item;
                _isUpdatingUI = false;
            }
        }
    }

    public class AppItem
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public int Volume { get; set; }
        public bool Muted { get; set; }

        public override string ToString()
        {
            string status = Muted ? "[MUDO]" : $"[{Volume}%]";
            return $"{status}  {Name}";
        }
    }
}