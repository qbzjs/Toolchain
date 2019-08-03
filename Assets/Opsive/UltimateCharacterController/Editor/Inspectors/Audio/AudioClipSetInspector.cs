﻿/// ---------------------------------------------
/// Ultimate Character Controller
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Opsive.UltimateCharacterController.Audio;
using System;
using System.Collections.Generic;
using Opsive.UltimateCharacterController.Editor.Inspectors.Utility;

namespace Opsive.UltimateCharacterController.Editor.Inspectors.Audio
{
    /// <summary>
    /// Draws a user friendly inspector for the AudioClipSet class.
    /// </summary>
    public static class AudioClipSetInspector
    {
        /// <summary>
        /// Draws the AudioClipSet.
        /// </summary>
        public static void DrawAudioClipSet(AudioClipSet audioClipSet, SerializedProperty serializedProperty, ref ReorderableList reorderableList, ReorderableList.ElementCallbackDelegate drawElementCallback,
                                                ReorderableList.AddCallbackDelegate addCallback, ReorderableList.RemoveCallbackDelegate removeCallback)
        {
            if (serializedProperty != null) {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(serializedProperty.FindPropertyRelative("m_Delay"));
                if (EditorGUI.EndChangeCheck()) {
                    serializedProperty.serializedObject.ApplyModifiedProperties();
                }
            } else {
                audioClipSet.Delay = EditorGUILayout.FloatField("Audio Delay", audioClipSet.Delay);
            }

            if (reorderableList == null || audioClipSet.AudioClips != reorderableList.list) {
                if (audioClipSet.AudioClips == null) {
                    audioClipSet.AudioClips = new AudioClip[0];
                }
                reorderableList = new ReorderableList(audioClipSet.AudioClips, typeof(AudioClip), true, true, true, true);
                reorderableList.drawHeaderCallback = OnAudioClipListHeaderDraw;
                reorderableList.drawElementCallback = drawElementCallback;
                reorderableList.onAddCallback = addCallback;
                reorderableList.onRemoveCallback = removeCallback;
                if (serializedProperty != null) {
                    reorderableList.serializedProperty = serializedProperty.FindPropertyRelative("m_AudioClips");
                }
            }
            // ReorderableLists do not like indentation.
            var indentLevel = EditorGUI.indentLevel;
            while (EditorGUI.indentLevel > 0) {
                EditorGUI.indentLevel--;
            }

            var listRect = GUILayoutUtility.GetRect(0, reorderableList.GetHeight());
            // Indent the list so it lines up with the rest of the content.
            listRect.x += InspectorUtility.IndentWidth * indentLevel;
            listRect.xMax -= InspectorUtility.IndentWidth * indentLevel;
            reorderableList.DoList(listRect);
            while (EditorGUI.indentLevel < indentLevel) {
                EditorGUI.indentLevel++;
            }
            GUILayout.Space(5);
        }

        /// <summary>
        /// Draws the header for the AudioClip list.
        /// </summary>
        private static void OnAudioClipListHeaderDraw(Rect rect)
        {
            EditorGUI.LabelField(rect, "Audio Clips");
        }

        /// <summary>
        /// Draws the AudioClip element.
        /// </summary>
        public static void OnAudioClipDraw(ReorderableList list, Rect rect, int index, AudioClipSet audioClipSet, UnityEngine.Object target)
        {
            EditorGUI.BeginChangeCheck();
            rect.y += 2;
            rect.height -= 5;
            if (list.serializedProperty != null) {
                list.serializedProperty.GetArrayElementAtIndex(index).objectReferenceValue = (AudioClip)EditorGUI.ObjectField(rect, audioClipSet.AudioClips[index], typeof(AudioClip), false);
                if (EditorGUI.EndChangeCheck()) {
                    list.serializedProperty.serializedObject.ApplyModifiedProperties();
                }
            } else {
                audioClipSet.AudioClips[index] = (AudioClip)EditorGUI.ObjectField(rect, audioClipSet.AudioClips[index], typeof(AudioClip), false);
                if (EditorGUI.EndChangeCheck() && target != null) {
                    InspectorUtility.RecordUndoDirtyObject(target, "Change Value");
                }
            }
        }

        /// <summary>
        /// Adds a new AudioClip element to the AudioClipSet.
        /// </summary>
        public static void OnAudioClipListAdd(ReorderableList list, AudioClipSet audioClipSet, UnityEngine.Object target)
        {
            if (list.serializedProperty != null) {
                list.serializedProperty.InsertArrayElementAtIndex(list.serializedProperty.arraySize);
                list.serializedProperty.serializedObject.ApplyModifiedProperties();
            } else {
                var audioClips = audioClipSet.AudioClips;
                if (audioClips == null) {
                    audioClips = new AudioClip[1];
                } else {
                    Array.Resize(ref audioClips, audioClips.Length + 1);
                }
                list.list = audioClipSet.AudioClips = audioClips;
                if (target != null) {
                    InspectorUtility.RecordUndoDirtyObject(target, "Change Value");
                }
            }
        }

        /// <summary>
        /// Remove the AudioClip element at the list index.
        /// </summary>
        public static void OnAudioClipListRemove(ReorderableList list, AudioClipSet audioClipSet, UnityEngine.Object target)
        {
            // Convert to a list and remove the audio clip. A new list needs to be assigned because a new allocation occurred.
            var audioClipList = new List<AudioClip>(audioClipSet.AudioClips);
            audioClipList.RemoveAt(list.index);
            if (list.serializedProperty != null) {
                list.serializedProperty.DeleteArrayElementAtIndex(list.index);
                list.serializedProperty.serializedObject.ApplyModifiedProperties();
            }
            list.list = audioClipSet.AudioClips = audioClipList.ToArray();
            list.index = list.index - 1;
            if (target != null) {
                InspectorUtility.RecordUndoDirtyObject(target, "Change Value");
            }
        }
    }
}