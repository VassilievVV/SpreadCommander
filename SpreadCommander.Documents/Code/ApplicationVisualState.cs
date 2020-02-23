using SpreadCommander.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Code
{
    [DataContract(Namespace = Parameters.ApplicationNamespace)]
    public class ApplicationVisualState
    {
        #region FormState
        [DataContract(Namespace = Parameters.ApplicationNamespace)]
        public class FormState
        {
            public FormState()
            {
            }

            public FormState(string name)
            {
                Name = name;
            }

            [DataMember()]
            public string Name { get; set; }

            [DataMember()]
            public int Width { get; set; }

            [DataMember()]
            public int Height { get; set; }

            public void Assign(FormState state)
            {
                Width  = state.Width;
                Height = state.Height;
            }
        }
        #endregion

        public static ApplicationVisualState Default { get; }

        static ApplicationVisualState()
        {
            Default = Load();
        }

        [DataMember()]
        public string SkinName { get; set; }

        [DataMember()]
        public List<FormState> FormStates { get; set; } = new List<FormState>();


        public static string SettingsFileName => Path.Combine(Parameters.ApplicationDataFolder, "AppVisualState.xml");

        private static ApplicationVisualState Load()
        {
            string settingsFilename = SettingsFileName;

            try
            {
                var settings = Utils.DeserializeObjectFromFile<ApplicationVisualState>(settingsFilename);
                return settings;
            }
            catch (Exception)
            {
                return new ApplicationVisualState();
            }
        }

        public static void SaveSettings(ApplicationVisualState settings)
        {
            try
            {
                string settingsFilename = SettingsFileName;
                Utils.SerializeObjectToFile<ApplicationVisualState>(settings, settingsFilename);
            }
            catch (Exception)
            {
                //Do nothing
            }
        }

        public void SaveSettings() => SaveSettings(this);

        public FormState FindFormState(string name)
        {
            if (FormStates == null)
                return null;

            var result =
                (from formState in FormStates
                 where string.Compare(formState.Name, name, true) == 0
                 select formState).FirstOrDefault();
            return result;
        }

        public void SaveFormState(FormState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            var formState = FindFormState(state.Name);
            if (formState != null) formState.Assign(state);
            else FormStates.Add(state);
        }
    }
}
