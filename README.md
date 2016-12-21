# MinimizableGrid-for-Win8.1-Universal-and-UWP
The Control allows you to implement the Splitview functionality on windows 8.1 Universal and UWP application with increased 
flexibility with no use of external Nugget's or packages.
Features:
  1. Multiple direction Minimize functionality (Top, Bottom,Left,Right)
  2. Events to know when the operation is completed
  3. Handling speed of open and close via animation time property
  4. Adding differnt views to the header of the Grid to switch when in minimized view and when not

About the project flow:
Project is made on visual studio 2015 update 3 and the project is for UWP but the common class is portable to windows 8.1 Universal as well.
Select the mode of the demo from the Splitview
  1. First is a basic demo
  2. Second is the demo along with events and a sample header change
  3. Use the toggle switch on each view to toggle between horrizontal and vertical oriented Minimizable Grid


About the Properties:

1. CompactPaneHeightWidth -> Like the Splitview sets the visible portion when minimized. Defalut= 0
2. IsShowingMinimizedView -> gets the bool value that shows if the grid is minimized. Use this to bind header views to show when minimized.
3. IsMinimised -> Bool property to minimize or maximize the grid.
4. OldStateValue => Bool property to Hold the old IsMinimized value. just in case it's needed to restore the grid back to it's old state
        //for example: after popping it up during a search and bringing it back after search is finished.
5. AnimationTime -> Double Property to set the speed of the animation to fasten or slow down the transition
6. FlowOrientation -> Orientation Property to set the Grid Minimize Horrizontally or vertically.
7. MinimizeDirection -> Enum Property that helps to set the direction after FlowOrientation
      for ex: TowardsLeftOrTowardsTop would move the Grid towards left to minimize if FlowOrientation is Horrizontal and
              Towards Top if the FlowOrientation is vertical

And that's it.. it'll get you sarted for good. Do let me know if there's anything in the discussion section I look forward to coming across
new implementations for the control. Thanks 
