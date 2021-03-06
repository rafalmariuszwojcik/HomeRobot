﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Command
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class CommandParameterAttribute : Attribute
  {
    public CommandParameterAttribute(int index)
    {
      Index = index;
    }

    public int Index { get; }
  }

  public static class CommandFactory
  {
    private static IDictionary<string, Type> commandTypes = new Dictionary<string, Type>()
    {
      { "E00", typeof(EngineSpeedCommand)},
      { "DIST", typeof(RobotMoveCommand)},
      { "ENC", typeof(EncoderCommand)},
    };

    public static ICommand CreateCommand(string commandId, string[] parameters)
    {
      if (!commandTypes.ContainsKey(commandId))
      {
        return null;
      }

      var type = commandTypes[commandId];
      if (type == null)
      {
        return null;
      }

      var command = Activator.CreateInstance(type) as ICommand;
      var properties = command.GetType().GetProperties();
      foreach (var property in properties)
      {
        var attribute = property.GetCustomAttributes(false).OfType<CommandParameterAttribute>().FirstOrDefault();
        if (attribute != null)
        {
          var value = parameters.Length > attribute.Index ? parameters[attribute.Index] : null;
          if (value != null)
          {
            if (property.PropertyType == typeof(int))
            {
              property.SetValue(command, int.Parse(value), null);
            }
            else if (property.PropertyType == typeof(long))
            {
              property.SetValue(command, long.Parse(value), null);
            }
            else if (property.PropertyType == typeof(double))
            {
              property.SetValue(command, double.Parse(value), null);
            }
            else
            {
              property.SetValue(command, value, null);
            }
          }
        }
      }

      return command;
    }
  }
}