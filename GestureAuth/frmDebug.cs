using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestureAuth
{
    public partial class frmDebug : Form
    {
        public frmDebug()
        {
            InitializeComponent();
        }

        private void frmDebug_Load(object sender, EventArgs e)
        {
            tbSkinLowerLimit_1.Value = Config.skinLowerLimit_1;
            tbSkinLowerLimit_2.Value = Config.skinLowerLimit_2;
            tbSkinLowerLimit_3.Value = Config.skinLowerLimit_3;

            tbSkinUpperLimit_1.Value = Config.skinUpperLimit_1;
            tbSkinUpperLimit_2.Value = Config.skinUpperLimit_2;
            tbSkinUpperLimit_3.Value = Config.skinUpperLimit_3;

            tbThreshold_1.Value = Config.threshold_1;
            tbThreshold_2.Value = Config.threshold_2;
        }

        private void tbSkinLowerLimit_1_ValueChanged(object sender, EventArgs e)
        {
            Config.skinLowerLimit_1 = tbSkinLowerLimit_1.Value;
        }

        private void tbSkinLowerLimit_2_ValueChanged(object sender, EventArgs e)
        {
            Config.skinLowerLimit_2 = tbSkinLowerLimit_2.Value;
        }

        private void tbSkinLowerLimit_3_ValueChanged(object sender, EventArgs e)
        {
            Config.skinLowerLimit_3 = tbSkinLowerLimit_3.Value;
        }

        private void tbSkinUpperLimit_1_ValueChanged(object sender, EventArgs e)
        {
            Config.skinUpperLimit_1 = tbSkinUpperLimit_1.Value;
        }

        private void tbSkinUpperLimit_2_ValueChanged(object sender, EventArgs e)
        {
            Config.skinUpperLimit_2 = tbSkinUpperLimit_2.Value;
        }

        private void tbSkinUpperLimit_3_ValueChanged(object sender, EventArgs e)
        {
            Config.skinUpperLimit_3 = tbSkinUpperLimit_3.Value;
        }

        private void tbThreshold_1_ValueChanged(object sender, EventArgs e)
        {
            Config.threshold_1 = tbThreshold_1.Value;
        }

        private void tbThreshold_2_ValueChanged(object sender, EventArgs e)
        {
            Config.threshold_2 = tbThreshold_2.Value;
        }
    }
}
