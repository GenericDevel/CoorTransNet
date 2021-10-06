
// From: https://blog.csdn.net/Gou_Hailong/article/details/105362474?utm_medium=distribute.pc_relevant.none-task-blog-2~default~baidujs_title~default-1.no_search_link&spm=1001.2101.3001.4242

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoorTransNet
{
    /// <summary>
    /// Coordination transfer
    /// </summary>
    public class CoorTransfer
    {
        /* 此函数用于求取空间面七参数 未用到坐标重心化
         * 输入：po是源坐标,pt是目标坐标
         * 输出：resultMat是结果矩阵数组
         *     第一个矩阵 X      是所求的七参数
         *     第二个矩阵 V      是改正数,可以算内符合精度
         *     第三个矩阵 Sigma2 是验后单位权中误差的平方
         */

        /// <summary>
        /// Calculate seven parameters 
        /// </summary>
        /// <param name="po">origine points set</param>
        /// <param name="pt">target points set</param>
        /// <returns></returns>
        public static Matrix[] Calculte7Parameters(Point[] po, Point[] pt)
        {
            int n = po.Length;
            int r = 3 * n - 7;//多余观测数
            Matrix[] resultMat = new Matrix[3];
            Matrix B = new Matrix(3 * n, 7);
            Matrix L = new Matrix(3 * n, 1);
            for (int i = 0; i < n; i++)
            {
                B[3 * i, 0] = 1;
                B[3 * i, 1] = 0;
                B[3 * i, 2] = 0;
                B[3 * i, 3] = 0;
                B[3 * i, 4] = -po[i].Z;
                B[3 * i, 5] = po[i].Y;
                B[3 * i, 6] = po[i].X;
                B[3 * i + 1, 0] = 0;
                B[3 * i + 1, 1] = 1;
                B[3 * i + 1, 2] = 0;
                B[3 * i + 1, 3] = po[i].Z;
                B[3 * i + 1, 4] = 0;
                B[3 * i + 1, 5] = -po[i].X;
                B[3 * i + 1, 6] = po[i].Y;
                B[3 * i + 2, 0] = 0;
                B[3 * i + 2, 1] = 0;
                B[3 * i + 2, 2] = 1;
                B[3 * i + 2, 3] = -po[i].Y;
                B[3 * i + 2, 4] = po[i].X;
                B[3 * i + 2, 5] = 0;
                B[3 * i + 2, 6] = po[i].Z;
                L[3 * i, 0] = pt[i].X;
                L[3 * i + 1, 0] = pt[i].Y;
                L[3 * i + 2, 0] = pt[i].Z;
            }
            Matrix Nbb = B.Transpose() * B;
            Nbb.InvertGaussJordan();
            Matrix X = Nbb * B.Transpose() * L;
            Matrix V = B * X - L;
            Matrix Sigma2 = V.Transpose() * V;//验后单位权中误差
            resultMat[0] = X;
            resultMat[1] = V;
            resultMat[2] = Sigma2;
            return resultMat;
        }

        /* 此函数用于七参数坐标转换，对应函数为 Get_Seven
         * 输入：po是源坐标; M 是用七参数求取函数得到的矩阵数组
         * 输出：pt是目标坐标
         */

        /// <summary>
        /// Transfer the point with a origine coordination system to the target point with a new coordination system 
        /// </summary>
        /// <param name="po"> origine point</param>
        /// <param name="M"> seven parameter matrix </param>
        /// <returns>target point</returns>
        public static Point[] Transfer(Point[] po, Matrix[] M)
        {
            int n = po.Length;
            Point[] pt = new Point[n];
            Matrix X = M[0];
            Matrix B = new Matrix(3, 7);
            Matrix Pt;
            for (int i = 0; i < n; i++)
            {
                B[0, 0] = 1;
                B[0, 1] = 0;
                B[0, 2] = 0;
                B[0, 3] = 0;
                B[0, 4] = -po[i].Z;
                B[0, 5] = po[i].Y;
                B[0, 6] = po[i].X;
                B[0 + 1, 0] = 0;
                B[0 + 1, 1] = 1;
                B[1, 2] = 0;
                B[1, 3] = po[i].Z;
                B[1, 4] = 0;
                B[1, 5] = -po[i].X;
                B[1, 6] = po[i].Y;
                B[2, 0] = 0;
                B[2, 1] = 0;
                B[2, 2] = 1;
                B[2, 3] = -po[i].Y;
                B[2, 4] = po[i].X;
                B[2, 5] = 0;
                B[2, 6] = po[i].Z;
                Pt = B * X;
                pt[i] = new Point(Pt[0, 0], Pt[1, 0], Pt[2, 0]);
            }
            return pt;
        }
    }
}
