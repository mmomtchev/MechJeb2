/*
 * Copyright Lamont Granquist (lamont@scriptkiddie.org)
 * Dual licensed under the MIT (MIT-LICENSE) license
 * and GPLv2 (GPLv2-LICENSE) license or any later version.
 */

using System.Collections.Generic;
using MechJebLib.Maths;
using MechJebLib.Utils;

#nullable enable

namespace MechJebLib.Structs
{
    public class H1 : HBase<double>
    {
        private static readonly ObjectPool<H1> _pool = new ObjectPool<H1>(New);

        private static H1 New()
        {
            return new H1();
        }

        public static H1 Get()
        {
            H1 h = _pool.Get();
            return h;
        }

        public override void Dispose()
        {
            base.Dispose();
            _pool.Return(this);
        }

        protected override double Allocate()
        {
            return 0.0;
        }

        protected override double Allocate(double value)
        {
            return value;
        }

        protected override void Subtract(double a, double b, ref double result)
        {
            result = a - b;
        }

        protected override void Divide(double a, double b, ref double result)
        {
            result = a / b;
        }

        protected override void Multiply(double a, double b, ref double result)
        {
            result = a * b;
        }

        protected override void Addition(double a, double b, ref double result)
        {
            result = a + b;
        }

        protected override double Interpolant(double x1, double y1, double yp1, double x2, double y2, double yp2, double x)
        {
            return Functions.CubicHermiteInterpolant(x1, y1, yp1, x2, y2, yp2, x);
        }
    }
}
