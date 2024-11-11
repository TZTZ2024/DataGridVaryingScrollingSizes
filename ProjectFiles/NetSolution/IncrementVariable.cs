#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NativeUI;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.DataLogger;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using FTOptix.EventLogger;
using FTOptix.ODBCStore;
#endregion

public class IncrementVariable : BaseNetLogic
{
    FTOptix.UI.ListBox lst;
    public override void Start()
    {
        var runDataLoggerCheckBox = LogicObject.Owner.Owner.Get<CheckBox>("runDataLogger");
        var activeVariable = runDataLoggerCheckBox.CheckedVariable;
        activeVariable.VariableChange += OnActiveVariableChanged;
    }

    private void OnActiveVariableChanged(object sender, VariableChangeEventArgs e) {
        if ((bool)e.NewValue) {
            lst = (FTOptix.UI.ListBox)Owner;
            refreshTask = new PeriodicTask(LogData, 750, LogicObject);
            refreshTask.Start();
        } else {
            refreshTask?.Dispose();
        }
    }

    public override void Stop()
    {
        refreshTask?.Dispose();
    }

    public void LogData() {
        var myVar = Project.Current.GetVariable("Model/VarIncrement");
        myVar.Value += 1;
        lst.Refresh();
    }

    private PeriodicTask refreshTask;
}

