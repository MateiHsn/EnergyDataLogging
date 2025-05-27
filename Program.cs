using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SolarEnergyManagement
{
  public partial class MainForm : Form
  {
    private string currentUser = "";
    private bool isAdmin = false;
    private const string ADMIN_PASSWORD = "admin123"; // Preset admin password
    private Panel loginPanel;
    private Panel dashboardPanel;
    private TextBox usernameTextBox;
    private TextBox passwordTextBox;
    private TextBox signupUsernameTextBox;
    private TextBox signupPasswordTextBox;
    private TextBox signupEmailTextBox;
    private ListBox dataLogListBox;
    private Label energyGeneratedLabel;
    private Label energyConsumedLabel;
    private Label batteryLevelLabel;

    private Size savedSize = new Size(1000, 700);
    private FormWindowState savedWindowState = FormWindowState.Normal;
    private FormBorderStyle savedBorderStyle = FormBorderStyle.Sizable;
    private bool savedTopMost = false;

    public MainForm()
    {
      InitializeComponent();
      CreateXMLFiles();
      ShowLoginPanel();
    }

    private void InitializeComponent()
    {
      this.Text = "Solar Energy Management System";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.FormBorderStyle = FormBorderStyle.Sizable;
      this.MaximizeBox = true;
      this.WindowState = FormWindowState.Normal;

      this.Resize += MainForm_Resize;
      this.KeyDown += MainForm_KeyDown;
      this.KeyPreview = true;
    }

    private void CreateXMLFiles()
    {
      // Create users.xml if it doesn't exist
      if (!File.Exists("users.xml"))
      {
        XDocument usersDoc = new XDocument(
            new XElement("Users")
        );
        usersDoc.Save("users.xml");
      }

      // Create energy_data.xml if it doesn't exist
      if (!File.Exists("energy_data.xml"))
      {
        XDocument dataDoc = new XDocument(
            new XElement("EnergyData")
        );
        dataDoc.Save("energy_data.xml");
      }
    }

    // Add these new event handlers for Enter key support
    private void LoginTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        LoginButton_Click(sender, e);
      }
    }

    private void SignupTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        SignupButton_Click(sender, e);
      }
    }

    // Modified ShowLoginPanel function to support Enter key
    private void ShowLoginPanel()
    {
      this.Controls.Clear();

      // Set fixed login panel dimensions (independent of dashboard size)
      this.Size = new Size(500, 650);
      this.MinimumSize = new Size(500, 650);
      this.MaximumSize = new Size(500, 650);
      this.WindowState = FormWindowState.Normal;
      this.FormBorderStyle = FormBorderStyle.FixedSingle; // Prevent resizing on login

      // Always center the form on screen
      this.StartPosition = FormStartPosition.Manual;
      Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea;
      this.Location = new Point(
        screenBounds.X + (screenBounds.Width - this.Width) / 2,
        screenBounds.Y + (screenBounds.Height - this.Height) / 2
      );

      loginPanel = new Panel();
      loginPanel.Dock = DockStyle.Fill;
      loginPanel.BackColor = SystemColors.Control;

      // Title
      Label titleLabel = new Label();
      titleLabel.Text = "Solar Energy Management";
      titleLabel.Font = new Font("Arial", 16, FontStyle.Bold);
      titleLabel.Size = new Size(400, 30);
      titleLabel.Location = new Point(50, 30);
      titleLabel.TextAlign = ContentAlignment.MiddleCenter;

      // Login section
      Label loginLabel = new Label();
      loginLabel.Text = "Login";
      loginLabel.Font = new Font("Arial", 12, FontStyle.Bold);
      loginLabel.Size = new Size(100, 25);
      loginLabel.Location = new Point(50, 80);

      Label usernameLabel = new Label();
      usernameLabel.Text = "Username:";
      usernameLabel.Size = new Size(80, 20);
      usernameLabel.Location = new Point(50, 115);

      usernameTextBox = new TextBox();
      usernameTextBox.Size = new Size(200, 25);
      usernameTextBox.Location = new Point(140, 112);
      usernameTextBox.KeyDown += LoginTextBox_KeyDown; // Add Enter key support

      Label passwordLabel = new Label();
      passwordLabel.Text = "Password:";
      passwordLabel.Size = new Size(80, 20);
      passwordLabel.Location = new Point(50, 145);

      passwordTextBox = new TextBox();
      passwordTextBox.Size = new Size(200, 25);
      passwordTextBox.Location = new Point(140, 142);
      passwordTextBox.PasswordChar = '*';
      passwordTextBox.KeyDown += LoginTextBox_KeyDown; // Add Enter key support

      Button loginButton = new Button();
      loginButton.Text = "Login";
      loginButton.Size = new Size(100, 35);
      loginButton.Location = new Point(140, 180);
      loginButton.Click += LoginButton_Click;

      // Admin Login section
      Label adminLabel = new Label();
      adminLabel.Text = "Admin Login";
      adminLabel.Font = new Font("Arial", 12, FontStyle.Bold);
      adminLabel.Size = new Size(120, 25);
      adminLabel.Location = new Point(50, 235);

      Button adminLoginButton = new Button();
      adminLoginButton.Text = "Admin Login";
      adminLoginButton.Size = new Size(100, 35);
      adminLoginButton.Location = new Point(140, 270);
      adminLoginButton.Click += AdminLoginButton_Click;

      // Signup section
      Label signupLabel = new Label();
      signupLabel.Text = "Sign Up";
      signupLabel.Font = new Font("Arial", 12, FontStyle.Bold);
      signupLabel.Size = new Size(100, 25);
      signupLabel.Location = new Point(50, 325);

      Label signupUsernameLabel = new Label();
      signupUsernameLabel.Text = "Username:";
      signupUsernameLabel.Size = new Size(80, 20);
      signupUsernameLabel.Location = new Point(50, 360);

      signupUsernameTextBox = new TextBox();
      signupUsernameTextBox.Size = new Size(200, 25);
      signupUsernameTextBox.Location = new Point(140, 357);
      signupUsernameTextBox.KeyDown += SignupTextBox_KeyDown; // Add Enter key support

      Label signupEmailLabel = new Label();
      signupEmailLabel.Text = "Email:";
      signupEmailLabel.Size = new Size(80, 20);
      signupEmailLabel.Location = new Point(50, 390);

      signupEmailTextBox = new TextBox();
      signupEmailTextBox.Size = new Size(200, 25);
      signupEmailTextBox.Location = new Point(140, 387);
      signupEmailTextBox.KeyDown += SignupTextBox_KeyDown; // Add Enter key support

      Label signupPasswordLabel = new Label();
      signupPasswordLabel.Text = "Password:";
      signupPasswordLabel.Size = new Size(80, 20);
      signupPasswordLabel.Location = new Point(50, 420);

      signupPasswordTextBox = new TextBox();
      signupPasswordTextBox.Size = new Size(200, 25);
      signupPasswordTextBox.Location = new Point(140, 417);
      signupPasswordTextBox.PasswordChar = '*';
      signupPasswordTextBox.KeyDown += SignupTextBox_KeyDown; // Add Enter key support

      Button signupButton = new Button();
      signupButton.Text = "Sign Up";
      signupButton.Size = new Size(100, 35);
      signupButton.Location = new Point(140, 455);
      signupButton.Click += SignupButton_Click;

      Button exitLoginButton = new Button();
      exitLoginButton.Text = "Exit";
      exitLoginButton.Size = new Size(100, 35);
      exitLoginButton.Location = new Point(140, 500);
      exitLoginButton.Click += ExitButton_Click;

      loginPanel.Controls.AddRange(new Control[] {
    titleLabel, loginLabel, usernameLabel, usernameTextBox,
    passwordLabel, passwordTextBox, loginButton, adminLabel, adminLoginButton,
    signupLabel, signupUsernameLabel, signupUsernameTextBox,
    signupEmailLabel, signupEmailTextBox, signupPasswordLabel,
    signupPasswordTextBox, signupButton, exitLoginButton
  });

      this.Controls.Add(loginPanel);
    }

    private bool IsValidEmail(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
        return false;

      // Check if email contains exactly one "@" symbol
      int atCount = email.Count(c => c == '@');
      if (atCount != 1)
        return false;

      // Split by "@" to get local and domain parts
      string[] parts = email.Split('@');
      if (parts.Length != 2)
        return false;

      string localPart = parts[0];
      string domainPart = parts[1];

      // Check if local part is not empty
      if (string.IsNullOrWhiteSpace(localPart))
        return false;

      // Check if domain part is not empty and contains at least one dot
      if (string.IsNullOrWhiteSpace(domainPart) || !domainPart.Contains('.'))
        return false;

      // Check if domain has content before and after the dot
      string[] domainParts = domainPart.Split('.');
      if (domainParts.Length < 2)
        return false;

      // Ensure no empty parts in domain
      foreach (string part in domainParts)
      {
        if (string.IsNullOrWhiteSpace(part))
          return false;
      }

      return true;
    }

    private void LoginButton_Click(object sender, EventArgs e)
    {
      if (ValidateLogin(usernameTextBox.Text, passwordTextBox.Text))
      {
        currentUser = usernameTextBox.Text;
        isAdmin = false;
        ShowDashboard();
      }
      else
      {
        MessageBox.Show("Invalid username or password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void AdminLoginButton_Click(object sender, EventArgs e)
    {
      AdminLoginForm adminForm = new AdminLoginForm();
      adminForm.TopMost = true; // Force dialog to stay on top
      if (adminForm.ShowDialog() == DialogResult.OK)
      {
        if (adminForm.Password == ADMIN_PASSWORD)
        {
          currentUser = "Administrator";
          isAdmin = true;
          ShowDashboard();
        }
        else
        {
          MessageBox.Show("Invalid admin password!", "Admin Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void SignupButton_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrWhiteSpace(signupUsernameTextBox.Text) ||
          string.IsNullOrWhiteSpace(signupEmailTextBox.Text) ||
          string.IsNullOrWhiteSpace(signupPasswordTextBox.Text))
      {
        MessageBox.Show("Please fill in all fields!", "Signup Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      // Validate email format
      if (!IsValidEmail(signupEmailTextBox.Text))
      {
        MessageBox.Show("Please enter a valid email address. Email must contain exactly one '@' symbol and a valid domain name.",
                       "Invalid Email Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      if (CreateUser(signupUsernameTextBox.Text, signupEmailTextBox.Text, signupPasswordTextBox.Text))
      {
        MessageBox.Show("Account created successfully! Please login.", "Signup Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        signupUsernameTextBox.Clear();
        signupEmailTextBox.Clear();
        signupPasswordTextBox.Clear();
      }
      else
      {
        MessageBox.Show("Username already exists!", "Signup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private bool ValidateLogin(string username, string password)
    {
      try
      {
        XDocument doc = XDocument.Load("users.xml");
        string hashedPassword = HashPassword(password);

        var user = doc.Descendants("User")
            .FirstOrDefault(u => u.Element("Username")?.Value == username &&
                                u.Element("Password")?.Value == hashedPassword);

        return user != null;
      }
      catch
      {
        return false;
      }
    }

    private bool CreateUser(string username, string email, string password)
    {
      try
      {
        XDocument doc = XDocument.Load("users.xml");

        // Check if username already exists
        var existingUser = doc.Descendants("User")
            .FirstOrDefault(u => u.Element("Username")?.Value == username);

        if (existingUser != null)
          return false;

        // Add new user
        string hashedPassword = HashPassword(password);
        XElement newUser = new XElement("User",
            new XElement("Username", username),
            new XElement("Email", email),
            new XElement("Password", hashedPassword),
            new XElement("CreatedDate", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"))
        );

        doc.Root.Add(newUser);
        doc.Save("users.xml");
        return true;
      }
      catch
      {
        return false;
      }
    }

    private string HashPassword(string password)
    {
      using (SHA256 sha256Hash = SHA256.Create())
      {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
          builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
      }
    }

    private void ShowDashboard()
    {
      this.Controls.Clear();

      // Remove size constraints from login panel first
      this.MinimumSize = new Size(900, 650);
      this.MaximumSize = new Size();
      this.FormBorderStyle = FormBorderStyle.Sizable;
      this.TopMost = false;

      // Get screen dimensions and set form to fill entire screen
      Rectangle screenBounds = Screen.PrimaryScreen.Bounds; // Use Bounds instead of WorkingArea for full screen
      this.WindowState = FormWindowState.Normal; // Set to Normal first
      this.Size = screenBounds.Size;
      this.Location = screenBounds.Location;
      this.WindowState = FormWindowState.Maximized; // Then maximize
      this.StartPosition = FormStartPosition.Manual;

      // Force the form to process the size change before creating controls
      this.Refresh();
      Application.DoEvents();

      dashboardPanel = new Panel();
      dashboardPanel.Dock = DockStyle.Fill;
      dashboardPanel.AutoScroll = true;
      dashboardPanel.BackColor = SystemColors.Control;

      // Top left buttons group - Use consistent margins from edges
      int topMargin = 20;
      int leftMargin = 20;
      int buttonSpacing = 90; // Space between buttons

      Button logoutButton = new Button();
      logoutButton.Text = "Logout";
      logoutButton.Size = new Size(80, 30);
      logoutButton.Location = new Point(leftMargin, topMargin);
      logoutButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      logoutButton.Click += LogoutButton_Click;

      Button fullscreenButton = new Button();
      fullscreenButton.Text = "Fullscreen"; // Always start with "Fullscreen" text
      fullscreenButton.Size = new Size(80, 30);
      fullscreenButton.Location = new Point(leftMargin + buttonSpacing, topMargin);
      fullscreenButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      fullscreenButton.Click += FullscreenButton_Click;

      // Header - Fixed margin from top buttons
      int headerTopMargin = topMargin + 40; // 40px below the buttons
      Label headerLabel = new Label();
      headerLabel.Text = $"Solar Energy Dashboard - Welcome {currentUser}";
      headerLabel.Font = new Font("Arial", 16, FontStyle.Bold);
      headerLabel.Size = new Size(600, 30);
      headerLabel.Location = new Point(leftMargin, headerTopMargin);
      headerLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;

      // Current Energy Status - Fixed position relative to header
      int statusTopMargin = headerTopMargin + 40; // 40px below header
      GroupBox statusGroup = new GroupBox();
      statusGroup.Text = "Current Status";
      statusGroup.Font = new Font("Arial", 10, FontStyle.Bold);
      statusGroup.Size = new Size(280, 200);
      statusGroup.Location = new Point(leftMargin, statusTopMargin);
      statusGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left;

      energyGeneratedLabel = new Label();
      energyGeneratedLabel.Text = "Energy Generated: 0 kWh";
      energyGeneratedLabel.Size = new Size(250, 20);
      energyGeneratedLabel.Location = new Point(15, 30);

      energyConsumedLabel = new Label();
      energyConsumedLabel.Text = "Energy Consumed: 0 kWh";
      energyConsumedLabel.Size = new Size(250, 20);
      energyConsumedLabel.Location = new Point(15, 55);

      batteryLevelLabel = new Label();
      batteryLevelLabel.Text = "Battery Level: 0%";
      batteryLevelLabel.Size = new Size(250, 20);
      batteryLevelLabel.Location = new Point(15, 80);

      Button addDataButton = new Button();
      addDataButton.Text = "Add Energy Reading";
      addDataButton.Size = new Size(150, 40);
      addDataButton.Location = new Point(15, 120);
      addDataButton.Click += AddDataButton_Click;

      statusGroup.Controls.AddRange(new Control[] {
energyGeneratedLabel, energyConsumedLabel, batteryLevelLabel, addDataButton
});

      // Admin Panel (only visible for admin) - Fixed position relative to status group
      GroupBox adminGroup = null;
      if (isAdmin)
      {
        int adminTopMargin = statusTopMargin + 210; // 210px below status group start (200px height + 10px spacing)
        adminGroup = new GroupBox();
        adminGroup.Text = "Admin Panel";
        adminGroup.Font = new Font("Arial", 10, FontStyle.Bold);
        adminGroup.Size = new Size(200, 200);
        adminGroup.Location = new Point(leftMargin, adminTopMargin);
        adminGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left;

        Button manageUsersButton = new Button();
        manageUsersButton.Text = "Manage Users";
        manageUsersButton.Size = new Size(160, 40);
        manageUsersButton.Location = new Point(15, 30);
        manageUsersButton.Click += ManageUsersButton_Click;

        Button addUserButton = new Button();
        addUserButton.Text = "Add User";
        addUserButton.Size = new Size(160, 40);
        addUserButton.Location = new Point(15, 80);
        addUserButton.Click += AddUserButton_Click;

        Button viewAllDataButton = new Button();
        viewAllDataButton.Text = "View All Data";
        viewAllDataButton.Size = new Size(160, 40);
        viewAllDataButton.Location = new Point(15, 130);
        viewAllDataButton.Click += ViewAllDataButton_Click;

        adminGroup.Controls.AddRange(new Control[] {
  manageUsersButton, addUserButton, viewAllDataButton
});
      }

      // Data Log - Positioned to the right of other groups with dynamic sizing
      int logLeftMargin = 320; // Fixed left position
      GroupBox logGroup = new GroupBox();
      logGroup.Text = "Energy Data Log (All Users)";
      logGroup.Font = new Font("Arial", 10, FontStyle.Bold);

      // Calculate available space for the log group using actual client size
      int rightMargin = 40; // Right margin from screen edge
      int bottomMargin = 60; // Bottom margin from screen edge

      // Use actual client size now that form has been properly sized
      int availableWidth = Math.Max(550, this.ClientSize.Width - logLeftMargin - rightMargin);
      int availableHeight = Math.Max(400, this.ClientSize.Height - statusTopMargin - bottomMargin);

      logGroup.Size = new Size(availableWidth, availableHeight);
      logGroup.Location = new Point(logLeftMargin, statusTopMargin);
      logGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left;

      dataLogListBox = new ListBox();
      dataLogListBox.Font = new Font("Consolas", 12);

      // Calculate listbox size based on log group size
      int listBoxWidth = Math.Max(520, availableWidth - 30);
      int listBoxHeight = Math.Max(320, availableHeight - 80);

      dataLogListBox.Size = new Size(listBoxWidth, listBoxHeight);
      dataLogListBox.Location = new Point(15, 25);
      dataLogListBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

      Button refreshButton = new Button();
      refreshButton.Text = "Refresh Data";
      refreshButton.Size = new Size(100, 30);
      refreshButton.Location = new Point(15, availableHeight - 45);
      refreshButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      refreshButton.Click += RefreshButton_Click;

      Button exportButton = new Button();
      exportButton.Text = "Export Data";
      exportButton.Size = new Size(100, 30);
      exportButton.Location = new Point(125, availableHeight - 45);
      exportButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      exportButton.Click += ExportButton_Click;

      logGroup.Controls.AddRange(new Control[] {
dataLogListBox, refreshButton, exportButton
});

      var controlsToAdd = new List<Control> {
logoutButton, fullscreenButton, headerLabel, statusGroup, logGroup
};

      if (adminGroup != null)
        controlsToAdd.Add(adminGroup);

      dashboardPanel.Controls.AddRange(controlsToAdd.ToArray());

      this.Controls.Add(dashboardPanel);
      LoadEnergyData();
    }


    private void ManageUsersButton_Click(object sender, EventArgs e)
    {
      UserManagementForm userForm = new UserManagementForm();
      userForm.TopMost = true; // Force dialog to stay on top
      userForm.ShowDialog();
    }

    private void AddUserButton_Click(object sender, EventArgs e)
    {
      AddUserForm addUserForm = new AddUserForm();
      addUserForm.TopMost = true; // Force dialog to stay on top
      if (addUserForm.ShowDialog() == DialogResult.OK)
      {
        if (CreateUser(addUserForm.Username, addUserForm.Email, addUserForm.Password))
        {
          MessageBox.Show("User created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
          MessageBox.Show("Username already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void ViewAllDataButton_Click(object sender, EventArgs e)
    {
      AllDataViewForm allDataForm = new AllDataViewForm();
      allDataForm.TopMost = true; // Force dialog to stay on top
      allDataForm.ShowDialog();
    }

    private void AddDataButton_Click(object sender, EventArgs e)
    {
      AddDataForm addForm = new AddDataForm();
      addForm.TopMost = true; // Force dialog to stay on top
      if (addForm.ShowDialog() == DialogResult.OK)
      {
        LogEnergyData(addForm.EnergyGenerated, addForm.EnergyConsumed, addForm.BatteryLevel);
        LoadEnergyData();
      }
    }

    private void RefreshButton_Click(object sender, EventArgs e)
    {
      LoadEnergyData();
    }

    private void ExportButton_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveDialog = new SaveFileDialog();
      saveDialog.Filter = "XML files (*.xml)|*.xml";
      saveDialog.Title = "Export Energy Data";
      saveDialog.FileName = $"energy_export_{DateTime.Now:yyyyMMdd_HHmmss}.xml";

      if (saveDialog.ShowDialog() == DialogResult.OK)
      {
        try
        {
          XDocument doc = XDocument.Load("energy_data.xml");
          var allEntries = doc.Descendants("Entry");

          XDocument exportDoc = new XDocument(
              new XElement("EnergyDataExport",
                  new XAttribute("ExportDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                  allEntries
              )
          );

          exportDoc.Save(saveDialog.FileName);
          MessageBox.Show("Data exported successfully!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Export failed: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void LogoutButton_Click(object sender, EventArgs e)
    {
      // Reset dashboard state to initial values instead of saving current state
      savedSize = new Size(1000, 700); // Reset to initial marker value
      savedWindowState = FormWindowState.Normal;
      savedBorderStyle = FormBorderStyle.Sizable;
      savedTopMost = false;

      currentUser = "";
      isAdmin = false;
      ShowLoginPanel();
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void CloseButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void MainForm_Resize(object sender, EventArgs e)
    {
      if (dashboardPanel != null && dashboardPanel.Visible)
      {
        UpdateDashboardLayout();
      }
    }

    private void UpdateDashboardLayout()
    {
      if (dashboardPanel == null) return;

      // Skip resizing when switching between fullscreen and windowed modes
      if (this.FormBorderStyle == FormBorderStyle.None && this.WindowState == FormWindowState.Maximized)
      {
        return; // In fullscreen mode
      }

      if (this.FormBorderStyle == FormBorderStyle.Sizable && this.WindowState == FormWindowState.Maximized)
      {
        return; // In windowed maximized mode
      }

      GroupBox logGroup = null;

      foreach (Control control in dashboardPanel.Controls)
      {
        if (control is GroupBox && control.Text == "Energy Data Log (All Users)")
        {
          logGroup = control as GroupBox;
          break;
        }
      }

      if (logGroup != null)
      {
        // Calculate available space with consistent margins
        // LogGroup starts at (320, 100), so right margin should be 320 and bottom margin should be 100
        int rightMargin = 320;
        int bottomMargin = 100;

        int availableWidth = Math.Max(520, this.ClientSize.Width - logGroup.Location.X - rightMargin);
        int availableHeight = Math.Max(320, this.ClientSize.Height - logGroup.Location.Y - bottomMargin);

        logGroup.Size = new Size(availableWidth, availableHeight);

        if (dataLogListBox != null && logGroup.Contains(dataLogListBox))
        {
          dataLogListBox.Size = new Size(availableWidth - 30, availableHeight - 80);
        }

        // Update button positions within log group
        foreach (Control control in logGroup.Controls)
        {
          if (control is Button)
          {
            Point currentLocation = control.Location;
            control.Location = new Point(currentLocation.X, availableHeight - 45);
          }
        }
      }
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
      // F11 for fullscreen toggle
      if (e.KeyCode == Keys.F11)
      {
        ToggleFullscreen();
      }
      // Alt+Enter for fullscreen toggle (alternative)
      else if (e.Alt && e.KeyCode == Keys.Enter)
      {
        ToggleFullscreen();
      }
    }

    private void FullscreenButton_Click(object sender, EventArgs e)
    {
      ToggleFullscreen();
    }

    private void ToggleFullscreen()
    {
      if (this.WindowState == FormWindowState.Maximized && this.FormBorderStyle == FormBorderStyle.None)
      {
        // Exit fullscreen - restore to maximized windowed mode
        this.FormBorderStyle = FormBorderStyle.Sizable;
        this.TopMost = false;

        // Restore to saved size (which should be the full screen bounds)
        Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
        this.Size = savedSize.Width > 0 ? savedSize : screenBounds.Size;
        this.Location = new Point(0, 0);
        this.WindowState = FormWindowState.Maximized;

        // Update fullscreen button text
        UpdateFullscreenButtonText("Fullscreen");
      }
      else
      {
        // Enter fullscreen - save current state first
        if (this.WindowState == FormWindowState.Maximized)
        {
          savedSize = Screen.PrimaryScreen.Bounds.Size;
        }

        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;
        this.TopMost = true;

        // Update fullscreen button text
        UpdateFullscreenButtonText("Windowed");
      }

      // Ensure layout is updated after fullscreen toggle
      if (dashboardPanel != null && dashboardPanel.Visible)
      {
        this.Refresh();
        UpdateDashboardLayout();

        // Ensure data is still loaded in the listbox
        if (dataLogListBox != null && dataLogListBox.Items.Count == 0)
        {
          LoadEnergyData();
        }
      }
    }

    private void UpdateFullscreenButtonText(string text)
    {
      if (dashboardPanel != null)
      {
        foreach (Control control in dashboardPanel.Controls)
        {
          if (control is Button && (control.Text == "Fullscreen" || control.Text == "Exit Fullscreen"))
          {
            control.Text = text;
            break;
          }
        }
      }
    }

    private void LogEnergyData(double generated, double consumed, double batteryLevel)
    {
      try
      {
        XDocument doc = XDocument.Load("energy_data.xml");

        XElement newEntry = new XElement("Entry",
            new XElement("User", currentUser),
            new XElement("Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            new XElement("EnergyGenerated", generated),
            new XElement("EnergyConsumed", consumed),
            new XElement("BatteryLevel", batteryLevel)
        );

        doc.Root.Add(newEntry);
        doc.Save("energy_data.xml");
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Failed to log data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void LoadEnergyData()
    {
      try
      {
        dataLogListBox.Items.Clear();
        XDocument doc = XDocument.Load("energy_data.xml");

        // Load ALL entries from ALL users, ordered by timestamp
        var allEntries = doc.Descendants("Entry")
            .OrderByDescending(e => e.Element("Timestamp")?.Value)
            .Take(20);

        // Calculate column widths based on content
        int timestampWidth = Math.Max(20, "Timestamp".Length);
        int userWidth = Math.Max(20, "User".Length);
        int generatedWidth = Math.Max(20, "Generated".Length);
        int consumedWidth = Math.Max(20, "Consumed".Length);
        int batteryWidth = Math.Max(20, "Battery".Length);

        // Calculate widths based on actual data
        foreach (var entry in allEntries)
        {
          string timestamp = entry.Element("Timestamp")?.Value ?? "";
          string user = entry.Element("User")?.Value ?? "";
          string generated = entry.Element("EnergyGenerated")?.Value ?? "0";
          string consumed = entry.Element("EnergyConsumed")?.Value ?? "0";
          string battery = entry.Element("BatteryLevel")?.Value ?? "0";

          timestampWidth = Math.Max(timestampWidth, timestamp.Length);
          userWidth = Math.Max(userWidth, user.Length);
          generatedWidth = Math.Max(generatedWidth, $"{generated}kWh".Length);
          consumedWidth = Math.Max(consumedWidth, $"{consumed}kWh".Length);
          batteryWidth = Math.Max(batteryWidth, $"{battery}%".Length);
        }

        // Add header row
        string headerTimestamp = CenterString("Timestamp", timestampWidth);
        string headerUser = CenterString("User", userWidth);
        string headerGenerated = CenterString("Generated", generatedWidth);
        string headerConsumed = CenterString("Consumed", consumedWidth);
        string headerBattery = CenterString("Battery", batteryWidth);

        dataLogListBox.Items.Add($"{headerTimestamp}|{headerUser}|{headerGenerated}|{headerConsumed}|{headerBattery}");
        dataLogListBox.Items.Add(new string('-', timestampWidth + userWidth + generatedWidth + consumedWidth + batteryWidth + 4)); // separator line

        foreach (var entry in allEntries)
        {
          string timestamp = entry.Element("Timestamp")?.Value ?? "";
          string user = entry.Element("User")?.Value ?? "";
          string generated = entry.Element("EnergyGenerated")?.Value ?? "0";
          string consumed = entry.Element("EnergyConsumed")?.Value ?? "0";
          string battery = entry.Element("BatteryLevel")?.Value ?? "0";

          // Format each column with calculated width and center alignment
          string formattedTimestamp = CenterString(timestamp, timestampWidth);
          string formattedUser = CenterString(user, userWidth);
          string formattedGenerated = CenterString($"{generated}kWh", generatedWidth);
          string formattedConsumed = CenterString($"{consumed}kWh", consumedWidth);
          string formattedBattery = CenterString($"{battery}%", batteryWidth);

          dataLogListBox.Items.Add($"{formattedTimestamp}|{formattedUser}|{formattedGenerated}|{formattedConsumed}|{formattedBattery}");
        }

        // Update current status with latest entry from any user
        var latestEntry = allEntries.FirstOrDefault();
        if (latestEntry != null)
        {
          energyGeneratedLabel.Text = $"Latest Energy Generated: {latestEntry.Element("EnergyGenerated")?.Value ?? "0"} kWh";
          energyConsumedLabel.Text = $"Latest Energy Consumed: {latestEntry.Element("EnergyConsumed")?.Value ?? "0"} kWh";
          batteryLevelLabel.Text = $"Latest Battery Level: {latestEntry.Element("BatteryLevel")?.Value ?? "0"}%";
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Failed to load data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private string CenterString(string text, int width)
    {
      if (text.Length >= width)
        return text;

      int padding = width - text.Length;
      int leftPadding = padding / 2;
      int rightPadding = padding - leftPadding;

      return new string(' ', leftPadding) + text + new string(' ', rightPadding);
    }
  }

  public partial class AdminLoginForm : Form
  {
    public string Password { get; private set; }
    private TextBox passwordTextBox;

    public AdminLoginForm()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      this.Text = "Admin Login";
      this.Size = new Size(300, 200);
      this.StartPosition = FormStartPosition.CenterParent;
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;

      Label titleLabel = new Label();
      titleLabel.Text = "Enter Admin Password";
      titleLabel.Font = new Font("Arial", 12, FontStyle.Bold);
      titleLabel.Size = new Size(200, 25);
      titleLabel.Location = new Point(50, 30);

      passwordTextBox = new TextBox();
      passwordTextBox.Size = new Size(200, 25);
      passwordTextBox.Location = new Point(50, 70);
      passwordTextBox.PasswordChar = '*';

      Button okButton = new Button();
      okButton.Text = "OK";
      okButton.Size = new Size(75, 30);
      okButton.Location = new Point(70, 110);
      okButton.DialogResult = DialogResult.OK;
      okButton.Click += OkButton_Click;

      Button cancelButton = new Button();
      cancelButton.Text = "Cancel";
      cancelButton.Size = new Size(75, 30);
      cancelButton.Location = new Point(155, 110);
      cancelButton.DialogResult = DialogResult.Cancel;

      this.Controls.AddRange(new Control[] {
                titleLabel, passwordTextBox, okButton, cancelButton
            });

      this.AcceptButton = okButton;
      this.CancelButton = cancelButton;
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
      Password = passwordTextBox.Text;
    }
  }

  public partial class UserManagementForm : Form
  {
    private ListBox usersListBox;

    public UserManagementForm()
    {
      InitializeComponent();
      LoadUsers();
    }

    private void InitializeComponent()
    {
      this.Text = "User Management";
      this.Size = new Size(500, 400);
      this.StartPosition = FormStartPosition.CenterParent;

      Label titleLabel = new Label();
      titleLabel.Text = "Manage Users";
      titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
      titleLabel.Size = new Size(200, 25);
      titleLabel.Location = new Point(20, 20);

      usersListBox = new ListBox();
      usersListBox.Size = new Size(440, 250);
      usersListBox.Location = new Point(20, 60);

      Button deleteUserButton = new Button();
      deleteUserButton.Text = "Delete Selected User";
      deleteUserButton.Size = new Size(150, 35);
      deleteUserButton.Location = new Point(20, 320);
      deleteUserButton.Click += DeleteUserButton_Click;

      Button refreshButton = new Button();
      refreshButton.Text = "Refresh";
      refreshButton.Size = new Size(100, 35);
      refreshButton.Location = new Point(180, 320);
      refreshButton.Click += RefreshButton_Click;

      Button closeButton = new Button();
      closeButton.Text = "Close";
      closeButton.Size = new Size(100, 35);
      closeButton.Location = new Point(290, 320);
      closeButton.Click += CloseButton_Click;

      this.Controls.AddRange(new Control[] {
                titleLabel, usersListBox, deleteUserButton, refreshButton, closeButton
            });
    }

    private void LoadUsers()
    {
      try
      {
        usersListBox.Items.Clear();
        XDocument doc = XDocument.Load("users.xml");

        var users = doc.Descendants("User");
        foreach (var user in users)
        {
          string username = user.Element("Username")?.Value ?? "";
          string email = user.Element("Email")?.Value ?? "";
          string createdDate = user.Element("CreatedDate")?.Value ?? "";
          usersListBox.Items.Add($"{username} - {email} - Created: {createdDate}");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Failed to load users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void DeleteUserButton_Click(object sender, EventArgs e)
    {
      if (usersListBox.SelectedIndex == -1)
      {
        MessageBox.Show("Please select a user to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      string selectedItem = usersListBox.SelectedItem.ToString();
      string username = selectedItem.Split(' ')[0]; // Extract username

      var result = MessageBox.Show($"Are you sure you want to delete user '{username}'?",
                                 "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

      if (result == DialogResult.Yes)
      {
        try
        {
          XDocument doc = XDocument.Load("users.xml");
          var userToDelete = doc.Descendants("User")
              .FirstOrDefault(u => u.Element("Username")?.Value == username);

          if (userToDelete != null)
          {
            userToDelete.Remove();
            doc.Save("users.xml");
            MessageBox.Show("User deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadUsers();
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Failed to delete user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void RefreshButton_Click(object sender, EventArgs e)
    {
      LoadUsers();
    }

    private void CloseButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }

  public partial class AddUserForm : Form
  {
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }

    private TextBox usernameTextBox;
    private TextBox emailTextBox;
    private TextBox passwordTextBox;

    public AddUserForm()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      this.Text = "Add New User";
      this.Size = new Size(350, 250);
      this.StartPosition = FormStartPosition.CenterParent;
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;

      Label titleLabel = new Label();
      titleLabel.Text = "Add New User";
      titleLabel.Font = new Font("Arial", 12, FontStyle.Bold);
      titleLabel.Size = new Size(200, 25);
      titleLabel.Location = new Point(20, 20);

      Label usernameLabel = new Label();
      usernameLabel.Text = "Username:";
      usernameLabel.Size = new Size(80, 20);
      usernameLabel.Location = new Point(20, 60);

      usernameTextBox = new TextBox();
      usernameTextBox.Size = new Size(200, 25);
      usernameTextBox.Location = new Point(110, 58);

      Label emailLabel = new Label();
      emailLabel.Text = "Email:";
      emailLabel.Size = new Size(80, 20);
      emailLabel.Location = new Point(20, 95);

      emailTextBox = new TextBox();
      emailTextBox.Size = new Size(200, 25);
      emailTextBox.Location = new Point(110, 93);

      Label passwordLabel = new Label();
      passwordLabel.Text = "Password:";
      passwordLabel.Size = new Size(80, 20);
      passwordLabel.Location = new Point(20, 130);

      passwordTextBox = new TextBox();
      passwordTextBox.Size = new Size(200, 25);
      passwordTextBox.Location = new Point(110, 128);
      passwordTextBox.PasswordChar = '*';

      Button okButton = new Button();
      okButton.Text = "Add User";
      okButton.Size = new Size(100, 35);
      okButton.Location = new Point(110, 170);
      okButton.DialogResult = DialogResult.OK;
      okButton.Click += OkButton_Click;

      Button cancelButton = new Button();
      cancelButton.Text = "Cancel";
      cancelButton.Size = new Size(75, 35);
      cancelButton.Location = new Point(220, 170);
      cancelButton.DialogResult = DialogResult.Cancel;

      this.Controls.AddRange(new Control[] {
                titleLabel, usernameLabel, usernameTextBox,
                emailLabel, emailTextBox, passwordLabel,
                passwordTextBox, okButton, cancelButton
            });

      this.AcceptButton = okButton;
      this.CancelButton = cancelButton;
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrWhiteSpace(usernameTextBox.Text) ||
          string.IsNullOrWhiteSpace(emailTextBox.Text) ||
          string.IsNullOrWhiteSpace(passwordTextBox.Text))
      {
        MessageBox.Show("Please fill in all fields!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        this.DialogResult = DialogResult.None;
        return;
      }

      // Validate email format
      if (!IsValidEmail(emailTextBox.Text))
      {
        MessageBox.Show("Please enter a valid email address. Email must contain exactly one '@' symbol and a valid domain name.",
                        "Invalid Email Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        this.DialogResult = DialogResult.None;
        return;
      }

      Username = usernameTextBox.Text;
      Email = emailTextBox.Text;
      Password = passwordTextBox.Text;
    }

    private bool IsValidEmail(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
        return false;

      // Check if email contains exactly one "@" symbol
      int atCount = email.Count(c => c == '@');
      if (atCount != 1)
        return false;

      // Split by "@" to get local and domain parts
      string[] parts = email.Split('@');
      if (parts.Length != 2)
        return false;

      string localPart = parts[0];
      string domainPart = parts[1];

      // Check if local part is not empty
      if (string.IsNullOrWhiteSpace(localPart))
        return false;

      // Check if domain part is not empty and contains at least one dot
      if (string.IsNullOrWhiteSpace(domainPart) || !domainPart.Contains('.'))
        return false;

      // Check if domain has content before and after the dot
      string[] domainParts = domainPart.Split('.');
      if (domainParts.Length < 2)
        return false;

      // Ensure no empty parts in domain
      foreach (string part in domainParts)
      {
        if (string.IsNullOrWhiteSpace(part))
          return false;
      }

      return true;
    }
  }

  // Modified AllDataViewForm class with delete functionality
  public partial class AllDataViewForm : Form
  {
    private ListBox allDataListBox;
    private List<XElement> dataEntries; // Store the actual XML elements

    public AllDataViewForm()
    {
      InitializeComponent();
      LoadAllData();
    }

    private void InitializeComponent()
    {
      this.Text = "All Energy Data";
      this.Size = new Size(900, 600); // Made wider to accommodate new button
      this.StartPosition = FormStartPosition.CenterParent;

      Label titleLabel = new Label();
      titleLabel.Text = "All Energy Data (All Users)";
      titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
      titleLabel.Size = new Size(300, 25);
      titleLabel.Location = new Point(20, 20);

      allDataListBox = new ListBox();
      allDataListBox.Font = new Font("Consolas", 9);
      allDataListBox.Size = new Size(850, 400); // Made wider
      allDataListBox.Location = new Point(20, 60);

      Button refreshButton = new Button();
      refreshButton.Text = "Refresh";
      refreshButton.Size = new Size(100, 35);
      refreshButton.Location = new Point(20, 480);
      refreshButton.Click += RefreshButton_Click;

      Button deleteButton = new Button();
      deleteButton.Text = "Delete Selected";
      deleteButton.Size = new Size(120, 35);
      deleteButton.Location = new Point(130, 480);
      deleteButton.Click += DeleteButton_Click;

      Button exportButton = new Button();
      exportButton.Text = "Export All";
      exportButton.Size = new Size(100, 35);
      exportButton.Location = new Point(260, 480);
      exportButton.Click += ExportButton_Click;

      Button closeButton = new Button();
      closeButton.Text = "Close";
      closeButton.Size = new Size(100, 35);
      closeButton.Location = new Point(370, 480);
      closeButton.Click += CloseButton_Click;

      this.Controls.AddRange(new Control[] {
      titleLabel, allDataListBox, refreshButton, deleteButton, exportButton, closeButton
    });
    }

    private void LoadAllData()
    {
      try
      {
        allDataListBox.Items.Clear();
        dataEntries = new List<XElement>(); // Clear the stored entries

        XDocument doc = XDocument.Load("energy_data.xml");

        var allEntries = doc.Descendants("Entry")
            .OrderByDescending(e => e.Element("Timestamp")?.Value)
            .ToList();

        foreach (var entry in allEntries)
        {
          string timestamp = entry.Element("Timestamp")?.Value ?? "";
          string user = entry.Element("User")?.Value ?? "";
          string generated = entry.Element("EnergyGenerated")?.Value ?? "0";
          string consumed = entry.Element("EnergyConsumed")?.Value ?? "0";
          string battery = entry.Element("BatteryLevel")?.Value ?? "0";

          string displayText = $"{timestamp} | User: {user} | Generated: {generated}kWh | Consumed: {consumed}kWh | Battery: {battery}%";
          allDataListBox.Items.Add(displayText);
          dataEntries.Add(entry); // Store the corresponding XML element
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Failed to load data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void DeleteButton_Click(object sender, EventArgs e)
    {
      if (allDataListBox.SelectedIndex == -1)
      {
        MessageBox.Show("Please select an entry to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      int selectedIndex = allDataListBox.SelectedIndex;
      string selectedItem = allDataListBox.SelectedItem.ToString();

      // Extract timestamp and user for confirmation
      string[] parts = selectedItem.Split('|');
      string timestamp = parts[0].Trim();
      string userPart = parts[1].Trim();

      var result = MessageBox.Show($"Are you sure you want to delete this entry?\n\n{timestamp}\n{userPart}",
                                 "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

      if (result == DialogResult.Yes)
      {
        try
        {
          // Get the corresponding XML element
          XElement entryToDelete = dataEntries[selectedIndex];

          // Remove from XML document
          entryToDelete.Remove();

          // Save the updated XML
          XDocument doc = XDocument.Load("energy_data.xml");
          doc.Save("energy_data.xml");

          MessageBox.Show("Entry deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
          LoadAllData(); // Refresh the list
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Failed to delete entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void RefreshButton_Click(object sender, EventArgs e)
    {
      LoadAllData();
    }

    private void ExportButton_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveDialog = new SaveFileDialog();
      saveDialog.Filter = "XML files (*.xml)|*.xml";
      saveDialog.Title = "Export Energy Data";
      saveDialog.FileName = $"energy_export_{DateTime.Now:yyyyMMdd_HHmmss}.xml";

      // Temporarily disable TopMost for main form if it's set
      bool wasTopMost = this.TopMost;
      if (wasTopMost) this.TopMost = false;

      if (saveDialog.ShowDialog() == DialogResult.OK)
      {
        try
        {
          XDocument doc = XDocument.Load("energy_data.xml");
          var allEntries = doc.Descendants("Entry");

          XDocument exportDoc = new XDocument(
              new XElement("EnergyDataExport",
                  new XAttribute("ExportDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                  allEntries
              )
          );

          exportDoc.Save(saveDialog.FileName);
          MessageBox.Show("Data exported successfully!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Export failed: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }

      // Restore TopMost state
      if (wasTopMost) this.TopMost = true;
    }

    private void CloseButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }

  public partial class AddDataForm : Form
  {
    public double EnergyGenerated { get; private set; }
    public double EnergyConsumed { get; private set; }
    public double BatteryLevel { get; private set; }

    private NumericUpDown generatedNumeric;
    private NumericUpDown consumedNumeric;
    private NumericUpDown batteryNumeric;

    public AddDataForm()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      this.Text = "Add Energy Reading";
      this.Size = new Size(350, 250);
      this.StartPosition = FormStartPosition.CenterParent;
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;

      Label titleLabel = new Label();
      titleLabel.Text = "Enter Energy Data";
      titleLabel.Font = new Font("Arial", 12, FontStyle.Bold);
      titleLabel.Size = new Size(200, 25);
      titleLabel.Location = new Point(20, 20);

      Label generatedLabel = new Label();
      generatedLabel.Text = "Energy Generated (kWh):";
      generatedLabel.Size = new Size(150, 20);
      generatedLabel.Location = new Point(20, 60);

      generatedNumeric = new NumericUpDown();
      generatedNumeric.Size = new Size(100, 25);
      generatedNumeric.Location = new Point(180, 58);
      generatedNumeric.DecimalPlaces = 2;
      generatedNumeric.Maximum = 9999;

      Label consumedLabel = new Label();
      consumedLabel.Text = "Energy Consumed (kWh):";
      consumedLabel.Size = new Size(150, 20);
      consumedLabel.Location = new Point(20, 95);

      consumedNumeric = new NumericUpDown();
      consumedNumeric.Size = new Size(100, 25);
      consumedNumeric.Location = new Point(180, 93);
      consumedNumeric.DecimalPlaces = 2;
      consumedNumeric.Maximum = 9999;

      Label batteryLabel = new Label();
      batteryLabel.Text = "Battery Level (%):";
      batteryLabel.Size = new Size(150, 20);
      batteryLabel.Location = new Point(20, 130);

      batteryNumeric = new NumericUpDown();
      batteryNumeric.Size = new Size(100, 25);
      batteryNumeric.Location = new Point(180, 128);
      batteryNumeric.Maximum = 100;

      Button okButton = new Button();
      okButton.Text = "OK";
      okButton.Size = new Size(75, 30);
      okButton.Location = new Point(150, 170);
      okButton.DialogResult = DialogResult.OK;
      okButton.Click += OkButton_Click;

      Button cancelButton = new Button();
      cancelButton.Text = "Cancel";
      cancelButton.Size = new Size(75, 30);
      cancelButton.Location = new Point(235, 170);
      cancelButton.DialogResult = DialogResult.Cancel;

      this.Controls.AddRange(new Control[] {
                titleLabel, generatedLabel, generatedNumeric,
                consumedLabel, consumedNumeric, batteryLabel,
                batteryNumeric, okButton, cancelButton
            });

      this.AcceptButton = okButton;
      this.CancelButton = cancelButton;
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
      EnergyGenerated = (double)generatedNumeric.Value;
      EnergyConsumed = (double)consumedNumeric.Value;
      BatteryLevel = (double)batteryNumeric.Value;
    }
  }

  static class Program
  {
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(true);
      Application.Run(new MainForm());
    }
  }
}