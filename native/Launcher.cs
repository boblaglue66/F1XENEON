using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Management;
using System.Collections.Generic;

class Launcher : Form {
    ComboBox displays = new ComboBox();
    Label status = new Label();
    Timer timer = new Timer();
    string profile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XeneonTV", "EdgeProfile");
    string settings = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XeneonTV", "display.txt");
    Screen target;
    HashSet<IntPtr> previous = new HashSet<IntPtr>();
    DateTime deadline;
    string pendingName;
    delegate bool EnumProc(IntPtr h, IntPtr p);
    [DllImport("user32.dll")] static extern bool EnumWindows(EnumProc callback, IntPtr param);
    [DllImport("user32.dll")] static extern bool IsWindowVisible(IntPtr h);
    [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr h, out uint id);
    [DllImport("user32.dll")] static extern bool SetWindowPos(IntPtr h, IntPtr after, int x, int y, int w, int height, uint flags);
    [DllImport("user32.dll")] static extern bool ShowWindow(IntPtr h, int command);
    [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr h);
    [STAThread] static void Main() { Application.EnableVisualStyles(); Application.SetCompatibleTextRenderingDefault(false); Application.Run(new Launcher()); }
    public Launcher() {
        Text = "XENEON TV — Edge"; Size = new Size(680,340); MinimumSize = new Size(580,320);
        BackColor = Color.FromArgb(12,17,25); ForeColor = Color.White; Font = new Font("Segoe UI",11);
        var layout = new TableLayoutPanel { Dock=DockStyle.Fill, Padding=new Padding(22), ColumnCount=2, RowCount=5 };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50)); layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute,40)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute,42)); layout.RowStyles.Add(new RowStyle(SizeType.Percent,100)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute,50)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute,30));
        var title = new Label { Text="XENEON TV  /  MICROSOFT EDGE", Dock=DockStyle.Fill, Font=new Font("Segoe UI",15,FontStyle.Bold) };
        layout.Controls.Add(title,0,0); layout.SetColumnSpan(title,2);
        displays.Dock=DockStyle.Fill; displays.DropDownStyle=ComboBoxStyle.DropDownList;
        string saved = File.Exists(settings) ? File.ReadAllText(settings) : "";
        int chosen=0;
        for(int i=0;i<Screen.AllScreens.Length;i++) {
            var s=Screen.AllScreens[i]; displays.Items.Add(s.DeviceName+" — "+s.Bounds.Width+" × "+s.Bounds.Height);
            if(s.Bounds.Width==2560 && s.Bounds.Height==720) chosen=i;
            if(s.DeviceName==saved) { chosen=i; break; }
        }
        // Populate all screens even when a saved display was found.
        displays.Items.Clear();
        foreach(var s in Screen.AllScreens) displays.Items.Add(s.DeviceName+" — "+s.Bounds.Width+" × "+s.Bounds.Height);
        displays.SelectedIndex=chosen;
        layout.Controls.Add(displays,0,1); layout.SetColumnSpan(displays,2);
        AddButton(layout,"F1 TV",0,Color.FromArgb(180,28,44),"https://f1tv.formula1.com/");
        AddButton(layout,"CANAL+",1,Color.FromArgb(40,57,77),"https://www.canalplus.com/");
        status.Text="Choisissez le XENEON EDGE dans la liste, puis un service."; status.Dock=DockStyle.Fill;
        layout.Controls.Add(status,0,3); layout.SetColumnSpan(status,2);
        var hint=new Label { Text="Connexion dans Edge • Profil conservé sur ce PC • Alt+F4 pour fermer", Dock=DockStyle.Fill, Font=new Font("Segoe UI",9), ForeColor=Color.LightGray };
        layout.Controls.Add(hint,0,4); layout.SetColumnSpan(hint,2); Controls.Add(layout);
        StartPosition=FormStartPosition.Manual; Bounds=Fit(Screen.AllScreens[chosen].WorkingArea);
        timer.Interval=600; timer.Tick += PlaceWindow;
    }
    Rectangle Fit(Rectangle area) { return new Rectangle(area.X+(area.Width-680)/2,area.Y+Math.Max(0,(area.Height-340)/2),680,340); }
    void AddButton(TableLayoutPanel panel,string name,int col,Color color,string url) {
        var b=new Button {Text=name,Dock=DockStyle.Fill,BackColor=color,ForeColor=Color.White,FlatStyle=FlatStyle.Flat,Font=new Font("Segoe UI",23,FontStyle.Bold),Margin=new Padding(4,10,4,10)};
        b.Click += delegate { Launch(name,url); }; panel.Controls.Add(b,col,2);
    }
    HashSet<uint> ProfileProcesses() {
        var ids=new HashSet<uint>();
        using(var query=new ManagementObjectSearcher("SELECT ProcessId, CommandLine FROM Win32_Process WHERE Name = 'msedge.exe'")) {
            foreach(ManagementObject process in query.Get()) {
                var command=process["CommandLine"] as string;
                if(command!=null && command.IndexOf(profile,StringComparison.OrdinalIgnoreCase)>=0) ids.Add(Convert.ToUInt32(process["ProcessId"]));
                process.Dispose();
            }
        }
        return ids;
    }
    HashSet<IntPtr> ProfileWindows() {
        var ids=ProfileProcesses(); var result=new HashSet<IntPtr>();
        EnumWindows(delegate(IntPtr h,IntPtr unused) { uint pid; GetWindowThreadProcessId(h,out pid); if(IsWindowVisible(h)&&ids.Contains(pid)) result.Add(h); return true; },IntPtr.Zero);
        return result;
    }
    void Launch(string name,string url) {
        if(timer.Enabled) return;
        try {
            string edge=null;
            foreach(var root in new[]{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}) {
                var candidate=Path.Combine(root,"Microsoft","Edge","Application","msedge.exe"); if(File.Exists(candidate)) {edge=candidate;break;}
            }
            if(edge==null) throw new Exception("Microsoft Edge est introuvable. Installez-le depuis le site Microsoft.");
            target=Screen.AllScreens[displays.SelectedIndex]; Directory.CreateDirectory(Path.GetDirectoryName(settings)); File.WriteAllText(settings,target.DeviceName);
            previous=ProfileWindows(); var r=target.WorkingArea;
            var args="--user-data-dir=\""+profile+"\" --app=\""+url+"\" --window-position="+r.X+","+r.Y+" --window-size="+r.Width+","+r.Height+" --no-first-run";
            Process.Start(new ProcessStartInfo(edge,args) {UseShellExecute=false});
            pendingName=name; deadline=DateTime.UtcNow.AddSeconds(20); status.Text="Ouverture de "+name+" dans Edge…"; timer.Start();
        } catch(Exception ex) { status.Text=ex.Message; }
    }
    void PlaceWindow(object sender, EventArgs e) {
        try {
            var windows=ProfileWindows();
            IntPtr handle=IntPtr.Zero;
            foreach(var h in windows) if(!previous.Contains(h)) {handle=h;break;}
            if(handle!=IntPtr.Zero) {
                var r=target.WorkingArea; ShowWindow(handle,9);
                if(!SetWindowPos(handle,IntPtr.Zero,r.X,r.Y,r.Width,r.Height,0x0004|0x0040)) throw new Exception("Impossible de placer la fenêtre Edge. Déplacez-la sur l'écran choisi.");
                SetForegroundWindow(handle); timer.Stop(); status.Text=pendingName+" ouvert sur "+target.DeviceName+". Connectez-vous dans Edge.";
            } else if(DateTime.UtcNow>deadline) {timer.Stop();status.Text="Edge lancé. Si la fenêtre est ailleurs, déplacez-la avec Windows + Maj + flèche.";}
        } catch(Exception ex) { timer.Stop(); status.Text=ex.Message; }
    }
}
