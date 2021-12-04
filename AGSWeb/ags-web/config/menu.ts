import { MenuItemType } from "../models/components/menuItemType";

const menus : MenuItemType[] = [
    {
      "id": "identity_admin",
      "labelKey": "menu_identity_admin_label",
      "functionClaim": "",
      "url": "/identity/",
      "childrenMenus": [
        {
          "id": "functionClaim_admin",
          "labelKey": "menu_functionClaim_admin_label",
          "functionClaim": "ags_identity_functionClaim_menu",
          "url": "/identity/functionclaims",
          "childrenMenus": []
        },
        {
          "id": "group_admin",
          "labelKey": "menu_group_admin_label",
          "functionClaim": "ags_identity_group_menu",
          "url": "/identity/groups",
          "childrenMenus": []
        },
        {
          "id": "user_admin",
          "labelKey": "menu_user_admin_label",
          "functionClaim": "ags_identity_user_menu",
          "url": "/identity/users",
          "childrenMenus": []
        },
        {
          "id": "change_password",
          "labelKey": "menu_change_password_label",
          "functionClaim": "ags_identity_user_change_password",
          "url": "/identity/changepassword",
          "childrenMenus": []
        }
      ]
    }
  ]
  
  
  
  export default menus;
  