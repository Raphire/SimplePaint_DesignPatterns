using Paint.Composite;
using Paint.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Paint.Commands
{
    class DecoratorCommand : ICommand<PaintSession>
    {
        Guid _drawableID;

        IDrawable _originalForm;
        readonly Dictionary<Ornament.Side, string> _newOrnaments;
        readonly Ornament.Side[] _removeOrnaments;

        public DecoratorCommand(Guid targetID, Dictionary<Ornament.Side, string> newOrnaments = null, params Ornament.Side[] removeOrnaments)
        {
            _drawableID = targetID;

            _newOrnaments = newOrnaments ?? new Dictionary<Ornament.Side, string>();
            _removeOrnaments = removeOrnaments;
        }

        public void Execute(PaintSession target)
        {
            IDrawable drawable = target.Canvas.FindNode(_drawableID);
            _originalForm = drawable;

            var parent = drawable.Parent;
            parent.Remove(_drawableID);
            
            var resultOrnaments = StripOrnaments(drawable, out IDrawable stripped);
            foreach(var side in _removeOrnaments) resultOrnaments.Remove(side);
            foreach(var kvp in _newOrnaments) resultOrnaments[kvp.Key] = kvp.Value;

            IDrawable newForm;
            
            if(drawable is IParentNode<IDrawable>)
            {
                newForm = resultOrnaments.Aggregate(stripped, (d, decSpec)
                    => new GroupOrnament(d as IParentNode<IDrawable>, decSpec.Value, decSpec.Key));
            } 
            else
            {
                newForm = resultOrnaments.Aggregate(stripped, (d, decSpec)
                    => new Ornament(d, decSpec.Value, decSpec.Key));
            }

            parent.Add(newForm);

            // Force redraw to show new ornament
            target.ClearSelection();
            target.Select(drawable);
        }

        public void Undo(PaintSession target)
        {
            IDrawable drawable = target.Canvas.FindNode(_drawableID);

            var parent = drawable.Parent;
            parent.Remove(_drawableID);

            parent.Add(_originalForm);
        }

        private Dictionary<Ornament.Side, string> StripOrnaments(IDrawable drawable, out IDrawable strippedDrawable)
        {
            var ornaments = new Dictionary<Ornament.Side, string>();

            while (drawable is Ornament ornament)
            {   // Save original Ornaments of target drawable
                drawable = ornament.Target;
                ornaments[ornament.DecoratedSide] = ornament.Text;
            }

            strippedDrawable = drawable;
            return ornaments;
        }
    }
}
