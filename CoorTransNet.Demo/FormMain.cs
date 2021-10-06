using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoorTransNet.Demo
{
    public partial class FormMain : Form
    {
        CoorTransNet.Point[] points_origine, points_target, points_unknown, points_converted;
        Matrix[] parameter;

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            this.points_converted = CoorTransfer.Transfer(this.points_unknown, this.parameter);

            for (int i = 0; i < this.dgvUnknownPoints.Rows.Count - 1; i++)
            {
                this.dgvUnknownPoints.Rows[i].Cells[3].Value=this.points_converted[i].X;
                this.dgvUnknownPoints.Rows[i].Cells[4].Value=this.points_converted[i].Y;
                this.dgvUnknownPoints.Rows[i].Cells[5].Value=this.points_converted[i].Z;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.CreateDemoPoints();
        }

        private void CreateDemoPoints()
        {
            this.dgvControlPoints.Rows.Add(1049422.40, 51089.20, 0, 121.622, -128.066, 0);
            this.dgvControlPoints.Rows.Add(1049413.95, 49659.30, 0, 141.228 ,187.718, 0);
            this.dgvControlPoints.Rows.Add(1049244.95, 49884.95, 0, 175.802 , 135.728, 0);
            

            this.dgvUnknownPoints.Rows.Add(1049187.361,51040.629,0,"-","-","-");
            this.dgvUnknownPoints.Rows.Add(1047637.713,51278.829, 0, "-", "-", "-");
            this.dgvUnknownPoints.Rows.Add(1046582.113,50656.241, 0, "-", "-", "-");
            this.dgvUnknownPoints.Rows.Add(1045644.713,49749.336, 0, "-", "-", "-");
        }

        private void btnCal7Parameters_Click(object sender, EventArgs e)
        {
            try
            {
                //Read the control points and save to origine and target points.
                this.points_origine = new CoorTransNet.Point[this.dgvControlPoints.Rows.Count-1];
                this.points_target = new CoorTransNet.Point[this.dgvControlPoints.Rows.Count-1];

                for (int i = 0; i < this.dgvControlPoints.Rows.Count-1; i++)
                {
                    var val = this.dgvControlPoints.Rows[i].Cells[0].Value;
                    this.points_origine[i] = new Point();
                    this.points_origine[i].X = (double)this.dgvControlPoints.Rows[i].Cells[0].Value;
                    this.points_origine[i].Y = double.Parse(this.dgvControlPoints.Rows[i].Cells[1].Value.ToString());
                    this.points_origine[i].Z = double.Parse(this.dgvControlPoints.Rows[i].Cells[2].Value.ToString());
                    this.points_target[i] = new Point();
                    this.points_target[i].X = double.Parse(this.dgvControlPoints.Rows[i].Cells[3].Value.ToString());
                    this.points_target[i].Y = double.Parse(this.dgvControlPoints.Rows[i].Cells[4].Value.ToString());
                    this.points_target[i].Z = double.Parse(this.dgvControlPoints.Rows[i].Cells[5].Value.ToString());
                }

                // Read the unknown points and save to array.
                this.points_unknown = new Point[this.dgvUnknownPoints.Rows.Count-1];
                for (int i = 0; i < this.dgvUnknownPoints.Rows.Count-1; i++)
                {
                    this.points_unknown[i] = new Point();
                    this.points_unknown[i].X = double.Parse(this.dgvUnknownPoints.Rows[i].Cells[0].Value.ToString());
                    this.points_unknown[i].Y = double.Parse(this.dgvUnknownPoints.Rows[i].Cells[1].Value.ToString());
                    this.points_unknown[i].Z = double.Parse(this.dgvUnknownPoints.Rows[i].Cells[2].Value.ToString());
                }

                Matrix[] para = CoorTransfer.Calculte7Parameters(points_origine, points_target);
                this.parameter = para;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(String.Format("dx = {0}\n\r", para[0].GetElement(0, 0)));
                sb.AppendLine(String.Format("dy = {0}\n\r", para[0].GetElement(1, 0)));
                sb.AppendLine(String.Format("dz = {0}\n\r", para[0].GetElement(2, 0)));
                sb.AppendLine(String.Format("rx = {0}\n\r", para[0].GetElement(3, 0)));
                sb.AppendLine(String.Format("ry = {0}\n\r", para[0].GetElement(4, 0)));
                sb.AppendLine(String.Format("rz = {0}\n\r", para[0].GetElement(5, 0)));
                sb.AppendLine(String.Format("dk = {0}\n\r", para[0].GetElement(6, 0)));
                sb.AppendLine("\n\r");
                sb.AppendLine("sigma2 = " + para[2].GetElement(0,0));

                this.txtLog.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }

        }
    }
}
