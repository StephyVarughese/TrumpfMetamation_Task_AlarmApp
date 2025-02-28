using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace TrumpfMetamation_Task_AlarmApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Process process = Process.Start("explorer", "ms-clock:");

            if (process == null)
            {
                Console.WriteLine("Failed to launch the Clock app.");
                return;
            }

            Console.WriteLine("Clock app launched successfully.");

            Thread.Sleep(3000);  // Wait for 3 seconds

            using (var automation = new UIA3Automation())
            {
                try
                {
                    var windows = automation.GetDesktop().FindAllChildren(c => c.ByControlType(ControlType.Window));

                    var mainWindow = windows.FirstOrDefault(w => w.Name.Contains("Clock"));

                    if (mainWindow == null)
                    {
                        Console.WriteLine("Failed to find the main Clock app window.");
                        return;
                    }

                    Console.WriteLine("Main window of the Clock app found.");

                    var alarmTab = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("AlarmButton")).AsButton();
                    alarmTab.Click();
                    Console.WriteLine("Navigated to Alarm tab.");

                    var addAlarmButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("AddAlarmButton")).AsButton();

                    addAlarmButton.WaitUntilClickable(TimeSpan.FromSeconds(5));  // Wait for 5 seconds

                    if (addAlarmButton != null && addAlarmButton.IsEnabled)
                    {
                        addAlarmButton.Click();
                        Console.WriteLine("Clicked on 'Add Alarm' button.");
                    }
                    else
                    {
                        Console.WriteLine("Could not find the 'Add Alarm' button.");
                        return; 
                    }

                    var hourList = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("hour")).AsComboBox();

                    if (hourList != null)
                    {
                        Console.WriteLine("Hour List found.");

                        var hourItem = hourList.Items.FirstOrDefault(item => item.Name.Contains("5"));  
                        if (hourItem != null)
                        {
                            hourItem.Select();
                            Console.WriteLine("Hour selected.");
                        }
                        else
                        {
                            Console.WriteLine("Hour item not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Could not find the Hour List.");
                    }

                    var minuteList = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("minute")).AsComboBox();

                    if (minuteList != null)
                    {
                        Console.WriteLine("Minute List found.");

                        var minuteItem = minuteList.Items.FirstOrDefault(item => item.Name.Contains("15"));
                        if (minuteItem != null)
                        {
                            minuteItem.Select();
                            Console.WriteLine("Minute selected.");
                        }
                        else
                        {
                            Console.WriteLine("Minute item not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Could not find the Minute List.");
                    }
                    var saveButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("SaveButton")).AsButton();
                    if (saveButton != null)
                    {
                        saveButton.Click();
                        Console.WriteLine("Alarm saved successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Save button not found.");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
