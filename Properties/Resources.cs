using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace SSTap.Properties
{
    [CompilerGenerated]
    [DebuggerNonUserCode]
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    internal class Resources
    {
        private static ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal Resources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (SSTap.Properties.Resources.resourceMan == null)
                    SSTap.Properties.Resources.resourceMan = new ResourceManager("SSTap.Properties.Resources", typeof(SSTap.Properties.Resources).Assembly);
                return SSTap.Properties.Resources.resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return SSTap.Properties.Resources.resourceCulture;
            }
            set
            {
                SSTap.Properties.Resources.resourceCulture = value;
            }
        }

        internal static Bitmap _000
        {
            get
            {
                return (Bitmap)SSTap.Properties.Resources.ResourceManager.GetObject(nameof(_000), SSTap.Properties.Resources.resourceCulture);
            }
        }

        internal static Bitmap abstract_user_flat_1
        {
            get
            {
                return (Bitmap)SSTap.Properties.Resources.ResourceManager.GetObject(nameof(abstract_user_flat_1), SSTap.Properties.Resources.resourceCulture);
            }
        }

        internal static Bitmap bg
        {
            get
            {
                return (Bitmap)SSTap.Properties.Resources.ResourceManager.GetObject(nameof(bg), SSTap.Properties.Resources.resourceCulture);
            }
        }

        internal static Bitmap globe_512
        {
            get
            {
                return (Bitmap)SSTap.Properties.Resources.ResourceManager.GetObject(nameof(globe_512), SSTap.Properties.Resources.resourceCulture);
            }
        }

        internal static Bitmap login_logo
        {
            get
            {
                return (Bitmap)SSTap.Properties.Resources.ResourceManager.GetObject(nameof(login_logo), SSTap.Properties.Resources.resourceCulture);
            }
        }

        internal static Bitmap main_header
        {
            get
            {
                return (Bitmap)SSTap.Properties.Resources.ResourceManager.GetObject(nameof(main_header), SSTap.Properties.Resources.resourceCulture);
            }
        }

        internal static Bitmap panel1
        {
            get
            {
                return (Bitmap)SSTap.Properties.Resources.ResourceManager.GetObject(nameof(panel1), SSTap.Properties.Resources.resourceCulture);
            }
        }

        internal static Bitmap witchlines_Simple_key
        {
            get
            {
                return (Bitmap)SSTap.Properties.Resources.ResourceManager.GetObject(nameof(witchlines_Simple_key), SSTap.Properties.Resources.resourceCulture);
            }
        }
    }
}
