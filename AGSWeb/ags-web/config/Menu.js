const menus = [
    {
      "Id": "identity_admin",
      "LabelKey": "menu_identity_admin_label",
      "FunctionClaim": "",
      "Url": "identity/",
      "ChildrenMenus": [
        {
          "Id": "functionClaim_admin",
          "LabelKey": "menu_functionClaim_admin_label",
          "FunctionClaim": "ags_identity_functionClaim_menu",
          "Url": "identity/functionclaims",
          "ChildrenMenus": []
        },
        {
          "Id": "group_admin",
          "LabelKey": "menu_group_admin_label",
          "FunctionClaim": "ags_identity_group_menu",
          "Url": "identity/groups",
          "ChildrenMenus": []
        },
        {
          "Id": "user_admin",
          "LabelKey": "menu_user_admin_label",
          "FunctionClaim": "ags_identity_user_menu",
          "Url": "identity/users",
          "ChildrenMenus": []
        },
        {
          "Id": "change_password",
          "LabelKey": "menu_change_password_label",
          "FunctionClaim": "",
          "Url": "identity/changepassword",
          "ChildrenMenus": []
        }
      ]
    }
  ]
  
  
  
  export default menus;
  