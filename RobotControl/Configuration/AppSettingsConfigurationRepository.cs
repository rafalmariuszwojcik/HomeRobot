namespace RobotControl.Configuration
{
  using System;
  using System.Configuration;
  using System.Globalization;
  using Configuration = System.Configuration.Configuration;

  /// <summary>
  /// Implementation that uses the app settings as configuration repository.
  /// </summary>
  public class AppSettingsConfigurationRepository : IConfigurationRepository
  {
    /// <summary>
    /// The application configuration file.
    /// </summary>
    private readonly Configuration configFile;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppSettingsConfigurationRepository"/> class.
    /// </summary>
    /// <exception cref="System.InvalidOperationException">If the config could not be loaded.</exception>
    public AppSettingsConfigurationRepository()
    {
      this.configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);

      ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
      fileMap.ExeConfigFilename = configFile.FilePath;
      Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
      this.configFile = configuration;

      if (this.configFile == null)
      {
        throw new InvalidOperationException(string.Format("Appsettings could not be accessed."));
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppSettingsConfigurationRepository"/> class.
    /// </summary>
    /// <param name="configuration">The configuration object.</param>
    public AppSettingsConfigurationRepository(Configuration configuration)
    {
      this.configFile = configuration;
    }

    /// <summary>
    /// Read a string valued key.
    /// </summary>
    /// <param name="section">The section to read <see cref="ident" /> from.</param>
    /// <param name="ident">The name of key to retrieve value from.</param>
    /// <param name="defaultValue">The default value to return when the key <see cref="ident" /> does not exist.</param>
    /// <returns>
    /// Value of <see cref="ident" /> as string.
    /// </returns>
    /// <remarks>
    ///   <see cref="ReadString" /> reads the key <see cref="ident" /> in section <see cref="section" />, and returns
    /// the value as a string. If the specified key or section do not exist, then the value in <see cref="defaultValue" />
    /// is returned. Note that if the key exists, but is empty, an empty string will be returned.
    /// </remarks>
    public string ReadString(string section, string ident, string defaultValue)
    {
      if (this.ValueExists(section, ident))
      {
        return this.configFile.AppSettings.Settings[this.GetSettingsKey(section, ident)].Value;
      }

      return defaultValue;
    }

    /// <summary>
    /// Writes a string value.
    /// </summary>
    /// <param name="section">The section to write key value to.</param>
    /// <param name="ident">The key name with which to write value.</param>
    /// <param name="value">The string value to write.</param>
    /// <remarks>
    ///   <see cref="WriteString" /> writes the string <see cref="value" /> with the name <see cref="ident" /> to the
    /// section <see cref="section" />, overwriting any previous value that may exist there. The section will be
    /// created if it does not exist.
    /// </remarks>
    public void WriteString(string section, string ident, string value)
    {
      var entry = this.GetEntry(section, ident);
      if (entry == null)
      {
        this.configFile.AppSettings.Settings.Add(new KeyValueConfigurationElement(this.GetSettingsKey(section, ident), value));
      }
      else
      {
        entry.Value = value;
      }
    }

    /// <summary>
    /// Read an integer value.
    /// </summary>
    /// <param name="section">The section to read <see cref="ident" /> from.</param>
    /// <param name="ident">The name of key to retrieve value from.</param>
    /// <param name="defaultValue">The default value to return when the key <see cref="ident" /> does not exist.</param>
    /// <returns>
    /// Value of <see cref="ident" /> as an integer.
    /// </returns>
    /// <remarks>
    ///   <see cref="ReadInteger" /> reads the key <see cref="ident" /> in section <see cref="section" />, and returns
    /// the value as an integer. If the specified key or section do not exist, then the value in <see cref="defaultValue" />
    /// is returned. If the key exists, but contains an invalid integer value, <see cref="defaultValue" /> is also returned.
    /// </remarks>
    public int ReadInteger(string section, string ident, int defaultValue)
    {
      int outValue;
      //return this.ReadString(section, ident, null).TryConvertTo(out outValue) ? outValue : defaultValue;
      return 0;
    }

    /// <summary>
    /// Writes an integer value.
    /// </summary>
    /// <param name="section">The section to write key value to.</param>
    /// <param name="ident">The key name with which to write value.</param>
    /// <param name="value">The integer value to write.</param>
    /// <remarks>
    ///   <see cref="WriteInteger" /> writes the integer <see cref="value" /> with the name <see cref="ident" /> to the
    /// section <see cref="section" />, overwriting any previous value that may exist there. The section will be
    /// created if it does not exist.
    /// </remarks>
    public void WriteInteger(string section, string ident, int value)
    {
      this.WriteString(section, ident, value.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Reads a long (int64) value.
    /// </summary>
    /// <param name="section">The section to read <see cref="ident" /> from.</param>
    /// <param name="ident">The name of key to retrieve value from.</param>
    /// <param name="defaultValue">The default value to return when the key <see cref="ident" /> does not exist.</param>
    /// <returns>
    /// Value of <see cref="ident" /> as a long.
    /// </returns>
    /// <remarks>
    /// <see cref="ReadLong" /> reads the key <see cref="ident" /> in section <see cref="section" />, and returns
    /// the value as a long. If the specified key or section do not exist, then the value in <see cref="defaultValue" />
    /// is returned. If the key exists, but contains an invalid long value, <see cref="defaultValue" /> is also returned.
    /// </remarks>
    public long ReadLong(string section, string ident, long defaultValue)
    {
      long outValue;
      //return ReadString(section, ident, null).TryConvertTo(out outValue) ? outValue : defaultValue;
      return 0;
    }

    /// <summary>
    /// Writes a long (int64) value.
    /// </summary>
    /// <param name="section">The section to write key value to.</param>
    /// <param name="ident">The key name with which to write value.</param>
    /// <param name="value">The long value to write.</param>
    /// <remarks>
    /// <see cref="WriteLong" /> writes the long <see cref="value" /> with the name <see cref="ident" /> to the
    /// section <see cref="section" />, overwriting any previous value that may exist there. The section will be
    /// created if it does not exist.
    /// </remarks>
    public void WriteLong(string section, string ident, long value)
    {
      this.WriteString(section, ident, value.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Read boolean value.
    /// </summary>
    /// <param name="section">The section to read <see cref="ident" /> from.</param>
    /// <param name="ident">The name of key to retrieve value from.</param>
    /// <param name="defaultValue">The default value to return when the key <see cref="ident" /> does not exist.</param>
    /// <returns>
    /// Value of <see cref="ident" /> as a boolean.
    /// </returns>
    /// <remarks>
    ///   <see cref="ReadBool" /> reads the key <see cref="ident" /> in section <see cref="section" />, and returns
    /// the value as an integer. If the specified key or section do not exist, then the value in <see cref="defaultValue" />
    /// is returned. If the key exists, but contains an invalid boolean value, <c>False</c> is returned.
    /// </remarks>
    public bool ReadBool(string section, string ident, bool defaultValue)
    {
      bool outValue;
      //return this.ReadString(section, ident, null).TryConvertTo(out outValue) ? outValue : defaultValue;
      return false;
    }

    /// <summary>
    /// Writes a boolean value.
    /// </summary>
    /// <param name="section">The section to write key value to.</param>
    /// <param name="ident">The key name with which to write value.</param>
    /// <param name="value">The boolean value to write.</param>
    /// <remarks>
    ///   <see cref="WriteBool" /> writes the boolean <see cref="value" /> with the name <see cref="ident" /> to the
    /// section <see cref="section" />, overwriting any previous value that may exist there. The section will be
    /// created if it does not exist.
    /// </remarks>
    public void WriteBool(string section, string ident, bool value)
    {
      this.WriteString(section, ident, value.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Read a Date/Time value.
    /// </summary>
    /// <param name="section">The section to read <see cref="ident" /> from.</param>
    /// <param name="ident">The name of key to retrieve value from.</param>
    /// <param name="defaultValue">The default value to return when the key <see cref="ident" /> does not exist.</param>
    /// <returns>
    /// Value of <see cref="ident" /> as a <see cref="DateTime" />.
    /// </returns>
    /// <remarks>
    ///   <see cref="ReadDateTime" /> reads the key <see cref="ident" /> in section <see cref="section" />, and returns
    /// the value as a date/time (<see cref="DateTime" />). If the specified key or section do not exist, then the value in <see cref="defaultValue" />
    /// is returned. If the key exists, but contains an invalid date/time value, <see cref="defaultValue" /> is also returned.
    /// </remarks>
    public DateTime ReadDateTime(string section, string ident, DateTime defaultValue)
    {
      DateTime outValue;
      //return this.ReadString(section, ident, null).TryConvertTo(out outValue) ? outValue : defaultValue;
      return DateTime.Now;
    }

    /// <summary>
    /// Writes a date/time value.
    /// </summary>
    /// <param name="section">The section to write key value to.</param>
    /// <param name="ident">The key name with which to write value.</param>
    /// <param name="value">The date/time value to write.</param>
    /// <remarks>
    ///   <see cref="WriteDateTime" /> writes the date/time <see cref="value" /> with the name <see cref="ident" /> to the
    /// section <see cref="section" />, overwriting any previous value that may exist there. The section will be
    /// created if it does not exist.
    /// </remarks>
    public void WriteDateTime(string section, string ident, DateTime value)
    {
      this.WriteString(section, ident, value.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Read an double value.
    /// </summary>
    /// <param name="section">The section to read <see cref="ident" /> from.</param>
    /// <param name="ident">The name of key to retrieve value from.</param>
    /// <param name="defaultValue">The default value to return when the key <see cref="ident" /> does not exist.</param>
    /// <returns>
    /// Value of <see cref="ident" /> as an double.
    /// </returns>
    /// <remarks>
    ///   <see cref="ReadDouble" /> reads the key <see cref="ident" /> in section <see cref="section" />, and returns
    /// the value as a double. If the specified key or section do not exist, then the value in <see cref="defaultValue" />
    /// is returned. If the key exists, but contains an invalid double value, <see cref="defaultValue" /> is also returned.
    /// </remarks>
    public double ReadDouble(string section, string ident, double defaultValue)
    {
      double outValue;
      //return this.ReadString(section, ident, null).TryConvertTo(out outValue) ? outValue : defaultValue;
      return 0.0;
    }

    /// <summary>
    /// Writes an double value.
    /// </summary>
    /// <param name="section">The section to write key value to.</param>
    /// <param name="ident">The key name with which to write value.</param>
    /// <param name="value">The double value to write.</param>
    /// <remarks>
    ///   <see cref="WriteDouble" /> writes the double <see cref="value" /> with the name <see cref="ident" /> to the
    /// section <see cref="section" />, overwriting any previous value that may exist there. The section will be
    /// created if it does not exist.
    /// </remarks>
    public void WriteDouble(string section, string ident, double value)
    {
      this.WriteString(section, ident, value.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Clear a section.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <remarks>
    ///   <see cref="DeleteSection" /> deletes all values from the section named <see cref="section" /> and removes the section.
    /// If the section didn't exists prior to a call to <see cref="DeleteSection" />, nothing happens.
    /// </remarks>
    public void DeleteSection(string section)
    {
    }

    /// <summary>
    /// Deletes a key from a section.
    /// </summary>
    /// <param name="section">The section from which to delete key.</param>
    /// <param name="ident">The name of the key to delete.</param>
    /// <remarks>
    ///   <see cref="DeleteKey" /> deletes the key <see cref="ident" /> from section <see cref="section" />.
    /// If the key or section didn't exist prior to the <see cref="DeleteKey" /> call, nothing happens.
    /// </remarks>
    public void DeleteKey(string section, string ident)
    {
      if (this.ValueExists(section, ident))
      {
        //this.configFile.Sections.Add("sectionName", new ConsoleSection());



        this.configFile.AppSettings.Settings.Remove(this.GetSettingsKey(section, ident));
      }
    }

    /// <summary>
    /// Clears all buffers for this configuration and causes any buffered data to be written to the underlying device.
    /// </summary>
    public void Flush()
    {
      this.configFile.Save(ConfigurationSaveMode.Modified);
    }

    /// <summary>
    /// Check if a value exists.
    /// </summary>
    /// <param name="section">The section in which to look for <see cref="ident" />.</param>
    /// <param name="ident">The key name to look for.</param>
    /// <returns>
    ///   <c>True</c> if <see cref="ident" /> exists in section <saftee cref="section" />.
    /// </returns>
    public bool ValueExists(string section, string ident)
    {
      return this.GetEntry(section, ident) != null;
    }

    /// <summary>
    /// Gets the settings key. The combination of section and ident separated by _.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <param name="ident">The ident.</param>
    /// <returns>The combination of section_ident.</returns>
    private string GetSettingsKey(string section, string ident)
    {
      return string.Format("{0}_{1}", section, ident);
    }

    /// <summary>
    /// Gets the entry by section_ident.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <param name="ident">The ident.</param>
    /// <returns>The key or null if it does not exist.</returns>
    private KeyValueConfigurationElement GetEntry(string section, string ident)
    {
      var name = this.GetSettingsKey(section, ident);
      return this.configFile.AppSettings.Settings[name];
    }
  }
}