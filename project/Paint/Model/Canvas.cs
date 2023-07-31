using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace Paint.Model
{
    public class Canvas : DrawableGroup, ISaveable
    {
        #region MEMBER FIELDS
        private readonly PictureBox _pictureBox;
        #endregion

        #region CONSTRUCTOR
        public Canvas(PictureBox pictureBox) 
            : base(new Point(0,0))
        {
            _id = Guid.Empty;
            _pictureBox = pictureBox;
            _pictureBox.Image = new Bitmap(pictureBox.ClientSize.Width, pictureBox.ClientSize.Height);
        }
        #endregion

        #region METHODS
        #region ISaveable
        public bool Save(string path)
        {
            try
            {
                File.WriteAllLines(path, this.DrawStrategy.ToString(this).Split('\n'));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Canvas could not be written to disk:");
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Load(string path)
        {
            try
            {
                PMLParser pmlParser = new PMLParser();
                
                this.Clear();
                this.Add(pmlParser.Parse(path).ToArray());

                return true;
            }
            catch (IOException ex)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(ex.Message);

                return false;
            }
            catch (FileFormatException ex)
            {
                Console.WriteLine("The file could not be parsed as a PML file:");
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        #endregion

        /// <summary>
        /// Exports canvas to a PNG file
        /// </summary>
        /// <param name="path">The path to write the PNG to</param>
        /// <returns>Whether or not the operation succeeded</returns>
        public bool Export(string path)
        {
            try
            {
                Bitmap bmpOut = new Bitmap(_pictureBox.ClientSize.Width, _pictureBox.ClientSize.Height);
                _pictureBox.DrawToBitmap(bmpOut, _pictureBox.ClientRectangle);

                bmpOut.Save(path);
                return true;
            } 
            catch(Exception ex)
            {
                Console.WriteLine("Ohnee alles kapot '{0}'", ex.Message);
                return false;
            }
        }
        #endregion
    }
}
