using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Qualcomm_Tools_GUI.My
{
    internal static partial class MyProject
    {
        internal partial class MyForms
        {

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Main m_Main;

            public Main Main
            {
                [DebuggerHidden]
                get
                {
                    m_Main = Create__Instance__(m_Main);
                    return m_Main;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Main))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Main);
                }
            }

        }


    }
}