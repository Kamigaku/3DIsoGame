using UnityEngine;
using System.IO;
using Utility;
using System;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

namespace AI {
    public class IAInterpretor : ScriptableObject {
        
        private object _caller;
        private object _previousResult = null;
        private Dictionary<string, IAFunction> functions = new Dictionary<string, IAFunction>();
        private string _nextFunction;

        // Use this for initialization
        public void InitiaiteAI(string fileName, object caller) {
            this._caller = caller;
            string cardDataJSON = File.ReadAllText("Assets/" + fileName);
            var jsonNodes = JSON.Parse(cardDataJSON);
            foreach (string functionNode in jsonNodes.GetKeys()) {
                Type type = Type.GetType(jsonNodes[functionNode]["targetType"]);
                if (type != null) {
                    MethodInfo method = type.GetMethod(jsonNodes[functionNode]["functionName"]);
                    if (method != null) {
                        object[] parameters = null;
                        bool[] parametersPrevious = null;
                        #region Paramètres
                        if (jsonNodes[functionNode]["parameters"] != null) {
                            parameters = new object[jsonNodes[functionNode]["parameters"].Count];
                            parametersPrevious = new bool[jsonNodes[functionNode]["parameters"].Count];
                            Type[] parametersType = new Type[jsonNodes[functionNode]["parameters"].Count];
                            string[] parametersValue = new string[jsonNodes[functionNode]["parameters"].Count];
                            string[] parametersOption = new string[jsonNodes[functionNode]["parameters"].Count];
                            for (int i = 0; i < jsonNodes[functionNode]["parameters"].Count; i++) {
                                parametersType[i] = Type.GetType(jsonNodes[functionNode]["parameters"][i]["type"]);
                                parametersOption[i] = jsonNodes[functionNode]["parameters"][i]["option"];
                                parametersValue[i] = jsonNodes[functionNode]["parameters"][i]["value"];
                            }
                            for (int i = 0; i < parametersType.Length; i++) {
                                switch (parametersOption[i]) {
                                    case "Defined as":
                                        parameters[i] = IAInterpretor.convertTo(parametersValue[i], parametersType[i]);
                                        parametersPrevious[i] = false;
                                        break;
                                    case "Current":
                                        parameters[i] = _caller;
                                        parametersPrevious[i] = false;
                                        break;
                                    case "From previous":
                                        parameters[i] = _previousResult;
                                        parametersPrevious[i] = true;
                                        break;
                                }
                            }
                        }
                        #endregion

                        if (this.functions.ContainsKey(method.Name)) {
                            throw new Exception("Two functions share the same name : " + method.Name);
                        }
                        if (jsonNodes[functionNode]["beginning"] != null) {
                            this.functions.Add(method.Name, new IAFunction(method, parameters, parametersPrevious, true));
                            this._nextFunction = method.Name;
                        }
                        else
                            this.functions.Add(method.Name, new IAFunction(method, parameters, parametersPrevious));

                        #region Réponses
                        if (jsonNodes[functionNode]["responses"] != null) {
                            for (int i = 0; i < jsonNodes[functionNode]["responses"].Count; i++) {
                                object value = IAInterpretor.convertTo(jsonNodes[functionNode]["responses"][i]["value"], Type.GetType(jsonNodes[functionNode]["returnType"]));
                                bool isEnd = false;
                                string target = string.Empty;
                                COMPARATOR comparator = COMPARATOR.NONE;
                                if (jsonNodes[functionNode]["responses"][i]["ending"] != null)
                                    isEnd = bool.Parse(jsonNodes[functionNode]["responses"][i]["ending"]);
                                if (jsonNodes[functionNode]["responses"][i]["option"] != null) {
                                    switch(jsonNodes[functionNode]["responses"][i]["option"]) {
                                        case "<":
                                            comparator = COMPARATOR.LESS;
                                            break;
                                        case "<=":
                                            comparator = COMPARATOR.LESS_OR_EQ;
                                            break;
                                        case ">":
                                            comparator = COMPARATOR.SUP;
                                            break;
                                        case ">=":
                                            comparator = COMPARATOR.SUP_OR_EQ;
                                            break;
                                        case "==":
                                            comparator = COMPARATOR.EQ;
                                            break;
                                        case "!=":
                                            comparator = COMPARATOR.NEQ;
                                            break;
                                        case "Default":
                                            comparator = COMPARATOR.DEFAULT;
                                            break;
                                    }
                                }
                                if(jsonNodes[functionNode]["responses"][i]["goTo"] != null) {
                                    target = jsonNodes[functionNode]["responses"][i]["goTo"];
                                }
                                functions[method.Name].AddResponse(new IAResponse(value, target, isEnd, comparator));
                            }
                        }
                        #endregion
                    }
                    else {
                        throw new Exception("Error in the AI, the methods doesn't exist !");
                    }
                }
            }
        }

        public void BeginIA(object caller) {
            bool found = false;
            do {
                this._previousResult = this.functions[this._nextFunction].Invoke(caller, this._previousResult);
                found = false;
                for (int i = 0; i < this.functions[this._nextFunction].responses.Count; i++) {
                    if (this.functions[this._nextFunction].responses[i].Compare(this._previousResult)) {
                        if(!this.functions[this._nextFunction].responses[i].isEnd)
                            this._nextFunction = this.functions[this._nextFunction].responses[i].target;
                        found = true;
                        break;
                    }
                }

            } while (found);
        }

        /// <summary>
        /// Converts a string to a desired type.
        /// </summary>
        /// <param name="from">The original string.</param>
        /// <param name="to">The type that you desire.</param>
        /// <returns>Return the string converted to your type</returns>
        public static object convertTo(string from, System.Type to) {
            if (to.IsPrimitive) {
                #region bool
                if(to == typeof(bool)) {
                    bool tempValue = false;
                    if (bool.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
                #region byte
                if (to == typeof(byte)) {
                    byte tempValue = 0;
                    if (byte.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
                #region sbyte
                else if (to == typeof(sbyte)) {
                    sbyte tempValue = 0;
                    if (sbyte.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
                #region char
                else if (to == typeof(char)) {
                    char tempValue = ' ';
                    if (char.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
                #region decimal
                else if (to == typeof(decimal)) {
                    decimal tempValue = 0;
                    if (decimal.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
                #region double
                else if (to == typeof(double)) {
                    double tempValue = 0;
                    if (double.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
                #region float
                else if (to == typeof(float)) {
                    float tempValue = 0;
                    if (float.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
                #region int
                else if (to == typeof(int)) {
                    int tempValue = 0;
                    if (int.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
                #region uint
                else if (to == typeof(uint)) {
                    uint tempValue = 0;
                    if (uint.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
                #region long
                else if (to == typeof(long)) {
                    long tempValue = 0;
                    if (long.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
                #region ulong
                else if (to == typeof(ulong)) {
                    ulong tempValue = 0;
                    if (ulong.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
                #region short
                else if (to == typeof(short)) {
                    short tempValue = 0;
                    if (short.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
                #region ushort
                else if (to == typeof(ushort)) {
                    ushort tempValue = 0;
                    if (ushort.TryParse(from, out tempValue)) return tempValue;
                }
                #endregion
            }
            else {
                if (to == typeof(GameObject)) {
                    return GameObject.Find(from);
                }
            }
            return null;
        }

    }

    internal class IAFunction {

        private MethodInfo _methodInfo;
        private object[] _parameters;
        private bool[] _isPrevious;
        private bool _isBeginning = false;
        private List<IAResponse> _responses;
        public bool isBeginning
        {
            get { return this._isBeginning; }
        }
        public List<IAResponse> responses
        {
            get { return this._responses; }
        }

        public IAFunction(MethodInfo methodInfo, object[] parameters, bool[] isPrevious, bool isBeginning = false) {
            this._methodInfo = methodInfo;
            this._parameters = parameters;
            this._isBeginning = isBeginning;
            this._isPrevious = isPrevious;
            this._responses = new List<IAResponse>();
        }

        public void AddResponse(IAResponse response) {
            this._responses.Add(response);
        }

        public object Invoke(object on = null, object previousResult = null) {
            for(int i = 0; i < _isPrevious.Length; i++) {
                if (_isPrevious[i])
                    _parameters[i] = previousResult;
            }
            return _methodInfo.Invoke(on, _parameters);
        }

    }

    internal class IAResponse {

        private object _response;
        private COMPARATOR _comparator;
        private bool _isEnd;
        private string _target;

        public bool isEnd {
            get {
                return _isEnd;
            }
        }

        public string target {
            get {
                return _target;
            }
        }

        public IAResponse(object response, string target = null, bool isEnd = false, COMPARATOR comparator = COMPARATOR.NONE) {
            this._response = response;
            this._isEnd = isEnd;
            this._comparator = comparator;
            this._target = target;
        }

        public bool Compare(object value) {
            if (value.GetType() == this._response.GetType() && value.GetType().IsPrimitive && this._response.GetType().IsPrimitive) {
                switch (this._comparator) {
                    case COMPARATOR.SUP:
                        if (Comparer.Default.Compare(value, this._response) > 0)
                            return true;
                        break;
                    case COMPARATOR.SUP_OR_EQ:
                        if (Comparer.Default.Compare(value, this._response) >= 0)
                            return true;
                        break;
                    case COMPARATOR.LESS:
                        if (Comparer.Default.Compare(value, this._response) < 0)
                            return true;
                        break;
                    case COMPARATOR.LESS_OR_EQ:
                        if (Comparer.Default.Compare(value, this._response) <= 0)
                            return true;
                        break;
                    case COMPARATOR.DEFAULT:
                        return true;
                    case COMPARATOR.EQ:
                        if (Comparer.Default.Compare(value, this._response) == 0)
                            return true;
                        break;
                    case COMPARATOR.NEQ:
                        if (Comparer.Default.Compare(value, this._response) != 0)
                            return true;
                        break;
                    case COMPARATOR.NONE:
                        return false;
                }
            }
            return false;
        }

    }

    internal enum COMPARATOR {
        SUP, SUP_OR_EQ, LESS, LESS_OR_EQ, EQ, NEQ, DEFAULT, NONE
    };
}