using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace project
{
	class Program
	{
		int docketsNum = -1;
		int rfpNum = -1;
		int repNum = -1;
		int durationNum = -1;
		//epoch timming
		double Epoch1 = 1359936000;
		double Epoch2 = 1362565743;
		


		private static string epochconv(double epoch)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch).ToLongDateString();
		}

		private List<List<Class1>> getList(string path1,
																  double initep,
																  double endep,
																  int dur
																)
		{
			//list of list values
			List<List<Class1>> Listm = new List<List<Class1>>();

			List<Class1> Week1 = new List<Class1>();
			List<Class1> Week2 = new List<Class1>();
			List<Class1> Week3 = new List<Class1>();
			List<Class1> Week4 = new List<Class1>();

			Class2 c2 = new Class2(path1, 1);
			int nr = c2.worksheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row;
			int nc = c2.worksheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Column;


			for (int i = 1; i <= nc; i++)
			{
				
				if (c2.FirstRow(1, i) == "Real First Packet")
					rfpNum = i;
				if (c2.FirstRow(1, i) == "Real End Packet")
					repNum = i;
				if (c2.FirstRow(1, i) == "doctets")
					docketsNum = i;
				if (c2.FirstRow(1, i) == "Duration")
					durationNum = i;
			}
			for (int i = 2; i < nr; i++)
			{
				

				double temp = c2.Reading1(i, rfpNum) / 1000;
				while (temp < endep && temp > initep)
				{
					if (temp > initep && temp < initep + dur)
					{

						if (
							 ((epochconv(temp)).Split(',')[0]) != "Saturday"
							&& ((epochconv(temp)).Split(',')[0]) != "Sunday")
						{

							if (int.Parse((epochconv(temp).Split(',')[1]).Split(' ')[2]) > 3 && int.Parse((epochconv(temp).Split(',')[1]).Split(' ')[2]) < 10)
							{
								Week1.Add(new Class1(
								 c2.Reading1(i, rfpNum),
								 c2.Reading1(i, repNum),
								 c2.Reading1(i, docketsNum),
								 c2.Reading1(i, durationNum)
								 ));
							}

							if (int.Parse((epochconv(temp).Split(',')[1]).Split(' ')[2]) > 10 && int.Parse((epochconv(temp).Split(',')[1]).Split(' ')[2]) < 17)
							{
								Week2.Add(new Class1(
								 c2.Reading1(i, rfpNum),
								 c2.Reading1(i, repNum),
								 c2.Reading1(i, docketsNum),
								 c2.Reading1(i, durationNum)
								 ));
							}

							if (int.Parse((epochconv(temp).Split(',')[1]).Split(' ')[2]) > 17 && int.Parse((epochconv(temp).Split(',')[1]).Split(' ')[2]) < 24)
							{
								Week3.Add(new Class1(
								 c2.Reading1(i, rfpNum),
								 c2.Reading1(i, repNum),
								 c2.Reading1(i, docketsNum),
								 c2.Reading1(i, durationNum)
								 ));
							}

							if (int.Parse((epochconv(temp).Split(',')[1]).Split(' ')[2]) > 24)
							{
								Week4.Add(new Class1(
								 c2.Reading1(i, rfpNum),
								 c2.Reading1(i, repNum),
								 c2.Reading1(i, docketsNum),
								 c2.Reading1(i, durationNum)
								 ));
							}


						}

						break;
					}

					else
					{

					}

					initep = initep + dur;
				}


			}

			Listm.Add(Week1);
			Listm.Add(Week2);
			Listm.Add(Week3);
			Listm.Add(Week4);


			return Listm;


		}
		//getting corelation values

		private double getCo(List<Class1> list1, List<Class1> list2)
		{
			int N = -1;
			double qx, qy, qxy;
			double qxs, qys;
			List<double> internetl1 = new List<double>();
			List<double> internetl2 = new List<double>();

			foreach (var item in list1)
			{
				internetl1.Add(item.internet);
			}

			foreach (var item in list2)
			{
				internetl2.Add(item.internet);
			}

			N = internetl1.Count;
			qx = 0; qy = 0; qxy = 0;
			qxs = 0; qys = 0;

			for (int i = 0; i < N - 1; i++)
			{
				qx = qx + internetl1[i];
				if (internetl2.Count - 1 < i)
					qy = qy + internetl2[i];
				if (internetl2.Count - 1 < i)
					qxy = qxy + internetl1[i] * internetl2[i];
				qxs = qxs + internetl1[i] * internetl1[i];
				if (internetl2.Count - 1 < i)
					qys = qys + internetl2[i] * internetl2[i];
			}

			double up = (N * qxy - qx * qy);
			double down = Math.Sqrt((N * qxs - ((qx) * (qx))) * (N * qys - (qy * qy)));

			return up / down;

		}

		//getting r values
		private double ZData(double rv)
		{
			double up = 1 + rv;
			double down = 1 - rv;
			return (Math.Log(up / down)) / 2;
		}

		private double HVal(double rv1, double rv2, double rv3)
		{
			double rms = (((rv1 * rv1) + (rv2 * rv2)) / 2);
			double fv = ((1 - rv3) / (2 * (1 - (rms))));
			return ((1 - (fv * rms)) / (1 - rms));
		}

		public double ZMain(double[] corcof, int n)
		{
			
			return ((ZData(corcof[0]) - ZData(corcof[2]))) *
						((Math.Sqrt((n - 3))) / ((2 * (1 - corcof[3])) * HVal(corcof[0], corcof[2], corcof[3])));
		}

		//Finally getting P value

		static double PValue(double z)
		{
			double p = 0.3275911;
			double a1 = 0.254829592;
			double a2 = -0.284496736;
			double a3 = 1.421413741;
			double a4 = -1.453152027;
			double a5 = 1.061405429;
			int sign;
			if (z < 0.0)
				sign = -1;
			else
				sign = 1;
			double x = Math.Abs(z) / Math.Sqrt(2.0);
			double t = 1.0 / (1.0 + p * x);
			double erf = 1.0 - (((((a5 * t + a4) * t) + a3)
			* t + a2) * t + a1) * t * Math.Exp(-x * x);
			return 0.5 * (1.0 + sign * erf);
		}

		static void Main(string[] args)
		{
			List<List<Class1>> lu1 = new List<List<Class1>>();
			List<List<Class1>> listsUser2 = new List<List<Class1>>();
			double[] corel = new double[4];

			Program p = new Program();
			string path1 = @"C:\Users\goyal\Documents\ajb9b3.xlsx";
			string path2 = @"C:\Users\goyal\Documents\ajdqnf.xlsx";

			Console.WriteLine("What you want to check??? - 10 . 227 , 300");
			int input = int.Parse(Console.ReadLine());

			lu1 = p.getList(path1, p.Epoch1, p.Epoch2, input);
			listsUser2 = p.getList(path2, p.Epoch1, p.Epoch2, input);


			corel[0] = p.getCo(lu1[0], lu1[1]);
			corel[1] = p.getCo(lu1[0], listsUser2[0]);
			corel[2] = p.getCo(lu1[0], listsUser2[1]);
			corel[3] = p.getCo(lu1[1], listsUser2[1]);

			double Z_Value = p.ZMain(corel, lu1[0].Count);

			Console.WriteLine(PValue(Z_Value));

			Console.ReadLine();
		}
	}
}
