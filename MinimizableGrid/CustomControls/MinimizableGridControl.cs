using MinimizableGrid.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace MinimizableGrid.CustomControls
{
    public class MinimizableGridControl : Grid
    {

        public MinimizableGridControl()
        {

        }


        private double animationTime = 0.5;

        private double PreviousHeightWidthForVisibility = 0;

        public MinimizableGridControl CurrentElement;

        public event EventHandler<UIElement> OperationCompleted;

        //sets the height of the visible pane when Minimized.
        public double CompactPaneHeightWidth
        {
            get { return (double)GetValue(CompactPaneHeightWidthProperty); }
            set { SetValue(CompactPaneHeightWidthProperty, value); }
        }

        public static readonly DependencyProperty CompactPaneHeightWidthProperty =
            DependencyProperty.Register("CompactPaneHeightWidth", typeof(double), typeof(MinimizableGridControl), new PropertyMetadata(0.0));


        //To make the panel Minimize away from the edges, use the ReqMinimizeWithVisibility
        public bool ReqMinimizeWithVisibility
        {
            get { return (bool)GetValue(ReqMinimizeWithVisibilityProperty); }
            set { SetValue(ReqMinimizeWithVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReqMinimizeWithVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReqMinimizeWithVisibilityProperty =
            DependencyProperty.Register("ReqMinimizeWithVisibility", typeof(bool), typeof(MinimizableGridControl), new PropertyMetadata(false));



        //Toggle's the Minimization for the grid
        public bool IsMinimized
        {
            get { return (bool)GetValue(IsMinimizedProperty); }
            set { SetValue(IsMinimizedProperty, value); }
        }

        public static readonly DependencyProperty IsMinimizedProperty =
            DependencyProperty.Register("IsMinimized", typeof(bool), typeof(MinimizableGridControl), new PropertyMetadata(default(bool), (o, e) => ((MinimizableGridControl)o).IsMinimizedChanged(o, e)));

        private void IsMinimizedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(this.IsMinimized)
            {
                if (this.FlowOrientation == Orientation.Vertical)
                    this.PreviousHeightWidthForVisibility = this.RenderSize.Height;
                else
                    this.PreviousHeightWidthForVisibility = this.RenderSize.Width;
            }
            if (ReqMinimizeWithVisibility)
            {
                PerformMinimizeWithVisibility(sender, e);
            }
            else
            {
                PerformSimpleMinizie(sender, e);
            }
        }

        //Double animation to change height and width to minimize with visibility
        private void PerformMinimizeWithVisibility(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Get the Child elemnts
            var Parent = sender as MinimizableGridControl;
            var childs = Parent.Children;

            var MinimizingChild = (childs.FirstOrDefault(x => x as Border != null && ((Border)x).Name == MinimizingChildName)) as Border;

            if(MinimizingChild!=null)
            {
                MinimizingChild.Height = MinimizingChild.ActualHeight;
                MinimizingChild.Width = MinimizingChild.ActualWidth;
            }
            

            DoubleAnimation anim;
            var isMinimized = (bool)e.NewValue;
            OldStateValue = (bool)e.OldValue;
            if (OldStateValue != isMinimized)
            {
                CurrentElement = (MinimizableGridControl)sender;

                if (IsMinimized)
                {
                    anim = new DoubleAnimation()
                    {
                        EnableDependentAnimation = true,
                        From = PreviousHeightWidthForVisibility,
                        To = CompactPaneHeightWidth,
                        Duration = new Duration(new TimeSpan(0, 0, 1)),
                        EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
                    };
                }
                else
                {
                    anim = new DoubleAnimation()
                    {
                        EnableDependentAnimation = true,
                        From = CompactPaneHeightWidth,
                        To = PreviousHeightWidthForVisibility,
                        Duration = new Duration(new TimeSpan(0, 0, 1)),
                        EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
                    };
                }

                Storyboard.SetTarget(anim, CurrentElement);
                if (FlowOrientation == Orientation.Vertical)
                    Storyboard.SetTargetProperty(anim, "Height");
                else
                    Storyboard.SetTargetProperty(anim, "Width");

                CurrentElement.SlideStoryBoard.Stop();
                CurrentElement.SlideStoryBoard = new Storyboard();
                CurrentElement.SlideStoryBoard.Children.Add(anim);
                CurrentElement.SlideStoryBoard.Begin();

            }
        }

        //Simple Minimize Translate x and y animation
        private void PerformSimpleMinizie(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var isMinimized = (bool)e.NewValue;
            OldStateValue = (bool)e.OldValue;
            var VisiblePaneHeightWidth = CompactPaneHeightWidth;
            if (OldStateValue != isMinimized)
            {
                CurrentElement = (MinimizableGridControl)sender;
                var PortionToHide = CurrentElement.ActualHeight;
                CurrentElement.RenderTransform = new Windows.UI.Xaml.Media.TranslateTransform() { Y = 0, X = 0 };

                var animation = new DoubleAnimationUsingKeyFrames();
                EasingDoubleKeyFrame keyFrame1 = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)), Value = PortionToHide - VisiblePaneHeightWidth };
                EasingDoubleKeyFrame keyFrame2 = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(animationTime)), Value = 0 };
                keyFrame2.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
                animation.KeyFrames.Add(keyFrame1);
                animation.KeyFrames.Add(keyFrame2);

                Storyboard.SetTarget(animation, CurrentElement);
                Storyboard.SetTargetProperty(animation, "(UIElement.RenderTransform).(TranslateTransform.Y)");
                if (FlowOrientation == Orientation.Horizontal)
                {
                    Storyboard.SetTargetProperty(animation, "(UIElement.RenderTransform).(TranslateTransform.X)");
                    PortionToHide = CurrentElement.ActualWidth;
                }
                CurrentElement.SlideStoryBoard.Stop();
                CurrentElement.SlideStoryBoard = new Storyboard();
                CurrentElement.SlideStoryBoard.Children.Add(animation);

                if (!isMinimized)
                {
                    //set the IsShowingMinimizedView to false as it's getting maximized.
                    this.IsShowingMinimizedView = false;
                    if (MinimizeDirection == MinimizeFlow.TowardsRightOrTowardsBottom)
                    {
                        animation.KeyFrames[0] = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)), Value = PortionToHide - VisiblePaneHeightWidth };
                        animation.KeyFrames[1] = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(animationTime)), Value = 0 };
                    }
                    else
                    {
                        animation.KeyFrames[0] = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)), Value = -PortionToHide + VisiblePaneHeightWidth };
                        animation.KeyFrames[1] = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(animationTime)), Value = 0 };
                    }
                    CurrentElement.SlideStoryBoard.Begin();
                }
                else
                {

                    if (MinimizeDirection == MinimizeFlow.TowardsRightOrTowardsBottom)
                    {
                        animation.KeyFrames[0] = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)), Value = 0 };
                        animation.KeyFrames[1] = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(animationTime)), Value = PortionToHide - VisiblePaneHeightWidth };
                    }
                    else
                    {
                        animation.KeyFrames[0] = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)), Value = 0 };
                        animation.KeyFrames[1] = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(animationTime)), Value = -PortionToHide + VisiblePaneHeightWidth };
                    }
                    CurrentElement.SlideStoryBoard.Begin();
                }
            }
        }




        public string MinimizingChildName
        {
            get { return (string)GetValue(MinimizingChildNameProperty); }
            set { SetValue(MinimizingChildNameProperty, value); }
        }

        public static readonly DependencyProperty MinimizingChildNameProperty =
            DependencyProperty.Register("MinimizingChildName", typeof(string), typeof(MinimizableGridControl), new PropertyMetadata(String.Empty));



        //To fetch the old values just in case it's needed to restore the grid back to it's old state
        //for example: after popping it up during a search and bringing it back after search is finished.
        public bool? OldStateValue
        {
            get { return (bool?)GetValue(OldStateValueProperty); }
            set { SetValue(OldStateValueProperty, value); }
        }

        public static readonly DependencyProperty OldStateValueProperty =
            DependencyProperty.Register("OldStateValue", typeof(bool?), typeof(MinimizableGridControl), new PropertyMetadata(null));


        //To change the speed for opening and closing...
        public double AnimationTime
        {
            get { return (double)GetValue(AnimationTimeProperty); }
            set { SetValue(AnimationTimeProperty, value); }
        }

        public static readonly DependencyProperty AnimationTimeProperty =
            DependencyProperty.Register("AnimationTime", typeof(double), typeof(MinimizableGridControl), new PropertyMetadata(default(double), (o, e) => ((MinimizableGridControl)o).AnimationTimeChanged(o, e)));

        private void AnimationTimeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            animationTime = (double)e.NewValue;
        }

        public bool IsShowingMinimizedView
        {
            get { return (bool)GetValue(IsShowingMinimizedViewProperty); }
            set { SetValue(IsShowingMinimizedViewProperty, value); }
        }

        public static readonly DependencyProperty IsShowingMinimizedViewProperty =
            DependencyProperty.Register("IsShowingMinimizedView", typeof(bool), typeof(MinimizableGridControl), new PropertyMetadata(false));


        //To allow horrizontal and vertical minimization animation
        public Orientation FlowOrientation
        {
            get { return (Orientation)GetValue(FlowOrientationProperty); }
            set { SetValue(FlowOrientationProperty, value); }
        }

        public static readonly DependencyProperty FlowOrientationProperty =
            DependencyProperty.Register("FlowOrientation", typeof(Orientation), typeof(MinimizableGridControl), new PropertyMetadata(Orientation.Vertical));


        //To set the Fly out direction
        public MinimizeFlow MinimizeDirection
        {
            get { return (MinimizeFlow)GetValue(MinimizeDirectionProperty); }
            set { SetValue(MinimizeDirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinimizeDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimizeDirectionProperty =
            DependencyProperty.Register("MinimizeDirection", typeof(MinimizeFlow), typeof(MinimizableGridControl), new PropertyMetadata(MinimizeFlow.TowardsRightOrTowardsBottom));

        //Common StoryBoard for the Grid
        private Storyboard _slideStoryBoard = new Storyboard();
        public Storyboard SlideStoryBoard
        {
            get { return _slideStoryBoard; }
            set { _slideStoryBoard = value; _slideStoryBoard.Completed += _slideStoryBoard_Completed; }
        }

        private void _slideStoryBoard_Completed(object sender, object e)
        {
            //Set the IsShowingMinimizedView to true on Animation completed when minimized as untill it's done the Grid is not yet minimized
            if (this.IsMinimized)
                this.IsShowingMinimizedView = true;

            //for c# 6.0 use this:
            OperationCompleted?.Invoke(this, CurrentElement);

            //for c# before 6.0 comment the above and  Uncomment this:
            //if(OperationCompleted!=null)
            //{
            //    OperationCompleted(this, CurrentElement);
            //}
        }
    }

    public enum MinimizeFlow
    {
        TowardsLeftOrTowardsTop,
        TowardsRightOrTowardsBottom,
    }
}
