using Paint.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Paint
{
    public class PMLParser
    {
        /// <summary>
        /// Attempts to Parse the IDrawable hierarchy represented in the file that 'path' points to.
        /// </summary>
        /// 
        /// <param name="path">
        /// Path to the file to parse from.
        /// </param>
        /// 
        /// <returns>
        /// A list of IDrawable instances that were represented in the file that 'path' points to.
        /// </returns>
        /// 
        /// <exception cref="System.IO.FileFormatException" >
        /// Thrown when PMLParser could not resolve the content of the file specified by 'path'  
        /// </exception>
        public List<IDrawable> Parse(string path)
        {
            // This list will store the child-nodes we parse for Canvas (root node)
            List<IDrawable> canvasContent = new List<IDrawable>();

            using (TextReader txtIn = new StreamReader(path))
            {
                // The root of a 'PML' is always a single group (The canvas)
                // It looks like: 'group [x] 0 0' where [x] represents
                // the amount of children the group has.
                //
                // No Drawable instance will be created for the header,
                // as an empty PaintSession already has a Canvas.
                string[] header = txtIn.ReadLine().Split(' ');

                int expectedChildNodes = int.Parse(header[1]);

                int originX = int.Parse(header[2]);
                int originY = int.Parse(header[3]);

                canvasContent.AddRange(ParseAmount(expectedChildNodes, txtIn));
            }

            return canvasContent;
        }

        /// <summary>
        /// Parses the specified amount of IDrawable instances at the current level in the hierarchy.
        /// IDrawable instances nested in the same group at the current level are counted as a single 'parse'.
        /// </summary>
        /// 
        /// <param name="amount">
        /// The amount of IDrawable instances to parse at txtIn's current depth in the PML hierarchy.
        /// </param>
        /// 
        /// <param name="txtIn">
        /// The TextReader instance used to parse from a file.
        /// </param>
        /// 
        /// <returns>
        /// An array of IDrawable instances with the requested size.
        /// </returns>
        private IDrawable[] ParseAmount(int amount, TextReader txtIn)
        {
            IDrawable[] result = new IDrawable[amount];

            for(int i = 0; i < amount; i++)
            {
                result[i] = ParseNext(txtIn);
            }

            return result;
        }

        /// <summary>
        /// Attempts to parse the next IDrawable instance printed by txtIn.
        /// (including its nested children if it is an instance of Group)
        /// </summary>
        /// 
        /// <param name="txtIn">
        /// The TextReader instance used to parse from a file.
        /// </param>
        /// 
        /// <returns>
        /// An instance of IDrawable as described by txtIn.
        /// </returns>
        private IDrawable ParseNext(TextReader txtIn)
        {
            string line = txtIn.ReadLine();

            if (line == null) 
                throw new FileFormatException("Unexpected end of file");

            string[] args = line.TrimStart('\t').Split(' ');

            Enum.TryParse(args[0], true, out ShapeType shapeType);

            switch(shapeType)
            {
                case ShapeType.Ellipse:
                case ShapeType.Rectangle:
                    // For regular shapes arguments are: '[ShapeType] [posX] [posY] [width] [height]' (5)
                    return ParseShape(args);

                case ShapeType.Group:
                    // For groups arguments are: '[ShapeType] [# of children] [posX] [posY]' (4)
                    return ParseGroup(args, txtIn);

                case ShapeType.Ornament:
                    return ParseOrnament(args, txtIn);

                default:
                    throw new FileFormatException("Unknown shapetype '" + shapeType + "'");
            }
        }

        /// <summary>
        /// Helper function used to parse a single Shape instance that is described in 'args'.
        /// </summary>
        /// 
        /// <param name="args">
        /// For regular shapes parameters are: '[ShapeType] [posX] [posY] [width] [height]' (5).
        /// </param>
        /// 
        /// <returns>
        /// A single instance of type 'Shape' as described in 'args'.
        /// </returns>
        private IDrawable ParseShape(string[] args)
        {
            if (args.Length != 5)
            {
                throw new ArgumentException(
                    "Invalid number of arguments provided for a shape ("
                    + args.Length + " != 5)");
            }

            ShapeType shapeType;
            int posX = 0, posY = 0, 
                width = 0, height = 0;

            // Check whether any error occurs during parsing
            bool success =
                Enum.TryParse(args[0], true, out shapeType) &&
                int.TryParse(args[1], out posX) &&
                int.TryParse(args[2], out posY) &&
                int.TryParse(args[3], out width) &&
                int.TryParse(args[4], out height);

            if (!success)
            {   // Throw exception if any parse operation didn't succeed
                throw new FileFormatException("Failed to parse shape parameters");
            }

            // Create new shape object from parsed data
            Shape shape = new Shape(
                shapeType,
                new Point(posX, posY),
                new Size(width, height)
            );

            return shape;
        }

        /// <summary>
        /// Helper function used to parse a single Group instance that is described 
        /// by argument 'args', including eventual nested child-nodes.
        /// </summary>
        /// 
        /// <param name="args">
        /// For regular shapes parameters are: '[ShapeType] [posX] [posY] [width] [height]' (5)
        /// </param>
        /// 
        /// <returns>
        /// A single instance of type 'Shape' as described in 'args'
        /// </returns>
        private IDrawable ParseGroup(string[] args, TextReader txtIn)
        {
            if (args.Length != 4)
            {
                throw new ArgumentException(
                    "Invalid number of arguments provided for a group (" 
                    + args.Length + " != 4)");
            }

            ShapeType shapeType;
            int expectedChildNodes = 0, 
                posX = 0, posY = 0;

            // Check whether any error occurs during parsing
            bool success =
                Enum.TryParse(args[0], true, out shapeType) &&
                int.TryParse(args[1], out expectedChildNodes) &&
                int.TryParse(args[2], out posX) &&
                int.TryParse(args[3], out posY);

            if (!success)
            {   // Throw exception if any parse operation didn't succeed
                throw new FileFormatException("Failed to parse group parameters");
            }

            if (shapeType != ShapeType.Group)
            {
                throw new ArgumentException("");
            }

            // Create new group object from parsed data
            DrawableGroup group = new DrawableGroup(
                new Point(posX, posY)
            );

            IDrawable[] children = ParseAmount(expectedChildNodes, txtIn);

            group.Add(children);

            return group;
        }

        private IDrawable ParseOrnament(string[] args, TextReader txtIn)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException(
                    "Invalid number of arguments provided for a ornament ("
                    + args.Length + " != 3)");
            }

            Ornament.Side side;
            string text = args[2];

            if (Enum.TryParse(args[1], true, out side))
            {   // Throw exception if any parse operation didn't succeed
                return new Ornament(ParseNext(txtIn), text, side);
            }

            throw new FileFormatException("Failed to parse group parameters");
        }
    }
}
