   M S B A M L   `   `   `  ����?  ;CGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:CGUI.ControlsCGUI.Controls  5   	       CGUI.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   )iclr-namespace:CGUI.Controls     5   	     � Title$&   Computational Geometry - Starter��5   	   $	��450��5   	   $	��600��5   	   .��5      � .+�5	   
   !  CGUI.Controls.DrawingCanvas  -   5          Name$ drawingCanvas��5	      $��10000��5
      $��10000��5      $��Center=�5      $��Center=�5       � 	Resources 5       �� Source�� 5      $ Styles/DrawingStyle.xaml?�6%   %    5        5   
   -   5        $ resultCanvas��5      $��10000��5      $��10000��5      $��Center=�5      $��Center=�5      $y�	Collapsed=�5         
IsViewOnly .  5      �� 5      $ Styles/ResultStyle.xaml?�6%   %    5      � 5   
   $	��255��6   $��95��5      $��	0,0,10,10q�5      $��Right=�5       $��Bottom=�5!      .+�5"      �� ���#8AAA
������� �� RadiusX$ 5��5$       �� RadiusY$ 5��5%      �� 5&      $��5q�6   .+�5'      �� .��6   View:5)      �� 5*      -   5        $��algorithmList��5*      5+      �� 5,      $
��Wrap=�6   $��3q�5-      �� 6(   $0�*Crush those points and segments real good.��6,   6a   5.      5/      �� 50   
   -   5        $��clearButton��50         customButtonTemplate#��[  51      $��60��52      $��28��53      $��Clear��54      $��Right=�55      $��Bottom=�56      $��
0 0 10 110q�57      58      �� 59   
   -   5         ��Name$ 
saveButton��59      #��[  5:      $��60��5;      $��28��5<      $
��Save��5=      $��Right=�5>      $��Bottom=�5?      $��
0,0,75,110q�5@      5A      �� 5B   
   -   5        $ 
loadButton��5B      #��[  5C      $��60��5D      $��28��5E      $
��Load��5F      $��Right=�5G      $��Bottom=�5H      $��0,0,140,110q�5I      5J      �� 5K   
   -   5        $ 
undoButton��5K      #��[  5L      $��60��5M      $��28��5N      $
��Undo��5O      $��Right=�5P      $��Bottom=�5Q      $��0,0,205,110q�5R      5S      5T      5U      