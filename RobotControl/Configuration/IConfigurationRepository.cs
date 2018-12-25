using System;

namespace RobotControl.Configuration
{
  public interface IConfigurationRepository
  {
    string ReadString(string section, string ident, string defaultValue);
    void WriteString(string section, string ident, string value);
    int ReadInteger(string section, string ident, int defaultValue);
    void WriteInteger(string section, string ident, int value);
    long ReadLong(string section, string ident, long defaultValue);
    void WriteLong(string section, string ident, long value);
    bool ReadBool(string section, string ident, bool defaultValue);
    void WriteBool(string section, string ident, bool value);
    DateTime ReadDateTime(string section, string ident, DateTime defaultValue);
    void WriteDateTime(string section, string ident, DateTime value);
    double ReadDouble(string section, string ident, double defaultValue);
    void WriteDouble(string section, string ident, double value);
    void DeleteSection(string section);
    void DeleteKey(string section, string ident);
    void Flush();
    bool ValueExists(string section, string ident);
  }
}