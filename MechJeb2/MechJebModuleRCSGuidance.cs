﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MuMech
{
    //When enabled, the ascent guidance module makes the purple navball target point
    //along the ascent path. The ascent path can be set via SetPath. The ascent guidance
    //module disables itself if the player selects a different target.
    public class MechJebModuleRCSGuidance : DisplayModule
    {
        public MechJebModuleRCSGuidance(MechJebCore core) : base(core) { }

        const string TARGET_NAME = "RCS Guidance";

        protected override void WindowGUI(int windowID)
        {
            GUILayout.BeginVertical();

            GUIStyle s = new GUIStyle(GUI.skin.label);
            s.normal.textColor = Color.red;
            GUILayout.Label("HIGHLY EXPERIMENTAL", s);

            if (GUILayout.Button("Reset thrusters")) core.rcs.ResetThrusters();

            core.rcs.smartTranslation = GUILayout.Toggle(core.rcs.smartTranslation, "Smart translation");

            // The following options are really only useful for debugging.
            //core.rcs.runSolver = GUILayout.Toggle(core.rcs.runSolver, "Run solver");
            //core.rcs.applyResult = GUILayout.Toggle(core.rcs.applyResult, "Apply result");
            core.rcs.interpolateThrottle = GUILayout.Toggle(core.rcs.interpolateThrottle, "Interpolate throttle");
            core.rcs.forceRecalculate = GUILayout.Toggle(core.rcs.forceRecalculate, "Force recalculation");
            core.rcs.multithreading = GUILayout.Toggle(core.rcs.multithreading, "Multithreading");

            GuiUtils.SimpleTextBox("Torque factor", core.rcs.tuningParamFactorTorque);
            GuiUtils.SimpleTextBox("Translate factor", core.rcs.tuningParamFactorTranslate);
            GuiUtils.SimpleTextBox("Waste factor", core.rcs.tuningParamFactorWaste);
            GuiUtils.SimpleTextBox("Waste threshold", core.rcs.tuningParamWasteThreshold);

            core.rcs.solverThread.solver.factorTorque       = core.rcs.tuningParamFactorTorque;
            core.rcs.solverThread.solver.factorTranslate    = core.rcs.tuningParamFactorTranslate;
            core.rcs.solverThread.solver.factorWaste        = core.rcs.tuningParamFactorWaste;
            core.rcs.solverThread.solver.wasteThreshold     = core.rcs.tuningParamWasteThreshold;

            GUILayout.Label("Debug info:");
            GUILayout.Label(String.Format("solver time: {0:F3} s", core.rcs.solverThread.timeSeconds));
            GUILayout.Label(String.Format("total thrust: {0:F3}", core.rcs.solverThread.solver.totalThrust));
            GUILayout.Label(String.Format("thrusters used: {0}", core.rcs.thrustersUsed));
            GUILayout.Label(String.Format("efficiency: {0:F3}%", core.rcs.solverThread.solver.efficiency * 100));
            GUILayout.Label(String.Format("extra torque: {0:F3}", core.rcs.solverThread.solver.extraTorque.magnitude));
            if (core.rcs.solverThread.statusString != null)
            {
                GUILayout.Label(String.Format("status: {0}", core.rcs.solverThread.statusString));
            }

            //if (GUILayout.Button("Start thread")) core.rcs.solverThread.start();
            //if (GUILayout.Button("Stop thread"))  core.rcs.solverThread.stop();

            if (core.rcs.smartTranslation)
            {
                core.rcs.users.Add(this);
            }
            else
            {
                core.rcs.users.Remove(this);
            }

            GUILayout.EndVertical();

            base.WindowGUI(windowID);
        }

        public override GUILayoutOption[] WindowOptions()
        {
            return new GUILayoutOption[]{ GUILayout.Width(240), GUILayout.Height(30) };
        }

        public override string GetName()
        {
            return TARGET_NAME;
        }
    }
}
