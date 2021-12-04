import { LocalStringType } from "../models/common/localStringType"

export const default_locale = {
    label: "Eng",
    value: "en_uk"
}

export const locale_Options = [
    default_locale,
    {
        label: "繁",
        value: "zh_hk"
    }
    
]

export const locale_cookie_name = "ags_locale"

export const locale_strings: LocalStringType[] =  [
    {
        key: "menu_identity_admin_label",
        pairs: [
            {
                language: "en_uk",
                value: "Identity Admin"
            },
            {
                language: "zh_hk",
                value: "身份管理"
            }
        ]
    },
    {
        key: "menu_functionClaim_admin_label",
        pairs: [
            {
                language: "en_uk",
                value: "Function Claim Admin"
            },
            {
                language: "zh_hk",
                value: "功能管理"
            }
        ]
    },
    {
        key: "menu_group_admin_label",
        pairs: [
            {
                language: "en_uk",
                value: "Group Admin"
            },
            {
                language: "zh_hk",
                value: "團體管理"
            }
        ]
    },
    {
        key: "menu_user_admin_label",
        pairs: [
            {
                language: "en_uk",
                value: "User Admin"
            },
            {
                language: "zh_hk",
                value: "用戶管理"
            }
        ]
    },
    {
        key: "menu_change_password_label",
        pairs: [
            {
                language: "en_uk",
                value: "Change Password"
            },
            {
                language: "zh_hk",
                value: "更改密碼"
            }
        ]
    },
    {
        key: "label_no_data_return",
        pairs: [
            {
                language: "en_uk",
                value: "No data has been returned"
            },
            {
                language: "zh_hk",
                value: "沒有數據返回"
            }
        ]
    },
    {
        key: "label_identity_user_table_username",
        pairs: [
            {
                language: "en_uk",
                value: "Username"
            },
            {
                language: "zh_hk",
                value: "用戶"
            }
        ]
    },
    {
        key: "label_identity_user_table_email",
        pairs: [
            {
                language: "en_uk",
                value: "Email"
            },
            {
                language: "zh_hk",
                value: "電郵"
            }
        ]
    }
    ,
    {
        key: "label_identity_user_table_first_name",
        pairs: [
            {
                language: "en_uk",
                value: "First Name"
            },
            {
                language: "zh_hk",
                value: "名字"
            }
        ]
    },
    {
        key: "label_identity_user_table_last_name",
        pairs: [
            {
                language: "en_uk",
                value: "Last Name"
            },
            {
                language: "zh_hk",
                value: "姓"
            }
        ]
    },
    {
        key: "label_identity_user_table_title",
        pairs: [
            {
                language: "en_uk",
                value: "Title"
            },
            {
                language: "zh_hk",
                value: "稱號"
            }
        ]
    },
    {
        key: "label_identity_user_table_groups",
        pairs: [
            {
                language: "en_uk",
                value: "Groups"
            },
            {
                language: "zh_hk",
                value: "團隊"
            }
        ]
    },
    {
        key: "label_identity_user_create_modal_title",
        pairs: [
            {
                language: "en_uk",
                value: "Create User"
            },
            {
                language: "zh_hk",
                value: "新增用户"
            }
        ]
    },
    {
        key: "label_identity_confirm_delete",
        pairs: [
            {
                language: "en_uk",
                value: "Confirm to delete"
            },
            {
                language: "zh_hk",
                value: "確定刪除"
            }
        ]
    },
    {
        key: "label_identity_group_table_name",
        pairs: [
            {
                language: "en_uk",
                value: "Name"
            },
            {
                language: "zh_hk",
                value: "名字"
            }
        ]
    },
    {
        key: "label_identity_group_table_functionClaim",
        pairs: [
            {
                language: "en_uk",
                value: "Function Claims"
            },
            {
                language: "zh_hk",
                value: "功能"
            }
        ]
    },
    {
        key: "label_identity_group_create_modal_title",
        pairs: [
            {
                language: "en_uk",
                value: "Create group"
            },
            {
                language: "zh_hk",
                value: "新增團體"
            }
        ]
    },
    {
        key: "label_identity_functionClaims_table_functionClaim",
        pairs: [
            {
                language: "en_uk",
                value: "Function Claim"
            },
            {
                language: "zh_hk",
                value: "功能"
            }
        ]
    },
    {
        key: "label_identity_functionClaims_table_description",
        pairs: [
            {
                language: "en_uk",
                value: "Description"
            },
            {
                language: "zh_hk",
                value: "描述"
            }
        ]
    },
    {
        key: "label_identity_functionClaims_create_modal_title",
        pairs: [
            {
                language: "en_uk",
                value: "Create Function Claim"
            },
            {
                language: "zh_hk",
                value: "新增功能"
            }
        ]
    },
    {
        key: "label_identity_changePassword_oldpassword",
        pairs: [
            {
                language: "en_uk",
                value: "Old Password"
            },
            {
                language: "zh_hk",
                value: "舊密碼"
            }
        ]
    },
    {
        key: "label_identity_changePassword_newpassword",
        pairs: [
            {
                language: "en_uk",
                value: "New Password"
            },
            {
                language: "zh_hk",
                value: "新密碼"
            }
        ]
    },
    {
        key: "label_identity_changePassword_newpassword2",
        pairs: [
            {
                language: "en_uk",
                value: "Re-enter new password"
            },
            {
                language: "zh_hk",
                value: "重複新密碼"
            }
        ]
    },
    {
        key: "error_identity_changePassword_password_mandatory",
        pairs: [
            {
                language: "en_uk",
                value: "Password is mandatory"
            },
            {
                language: "zh_hk",
                value: "密碼是必須"
            }
        ]
    },
    {
        key: "error_identity_changePassword_not_match_newpassword",
        pairs: [
            {
                language: "en_uk",
                value: "New passwords are different"
            },
            {
                language: "zh_hk",
                value: "新密碼不相同"
            }
        ]
    },
    {
        key: "label_common_save_succeeded",
        pairs: [
            {
                language: "en_uk",
                value: "Save succeeded"
            },
            {
                language: "zh_hk",
                value: "更新成功"
            }
        ]
    },
    {
        key: "label_common_save_failed",
        pairs: [
            {
                language: "en_uk",
                value: "Save failed"
            },
            {
                language: "zh_hk",
                value: "更新失敗"
            }
        ]
    },
    {
        key: "label_common_button_add",
        pairs: [
            {
                language: "en_uk",
                value: "Add"
            },
            {
                language: "zh_hk",
                value: "新增"
            }
        ]
    },
    {
        key: "label_common_button_delete",
        pairs: [
            {
                language: "en_uk",
                value: "Delete"
            },
            {
                language: "zh_hk",
                value: "刪除"
            }
        ]
    },
    {
        key: "label_common_button_confirm",
        pairs: [
            {
                language: "en_uk",
                value: "Confirm"
            },
            {
                language: "zh_hk",
                value: "確定"
            }
        ]
    },
    {
        key: "label_common_button_cancel",
        pairs: [
            {
                language: "en_uk",
                value: "Cancel"
            },
            {
                language: "zh_hk",
                value: "取消"
            }
        ]
    },
    {
        key: "label_common_button_action",
        pairs: [
            {
                language: "en_uk",
                value: "Action"
            },
            {
                language: "zh_hk",
                value: "操作"
            }
        ]
    },
    {
        key: "label_common_response_delete_succeeded",
        pairs: [
            {
                language: "en_uk",
                value: "Delete Succeeded"
            },
            {
                language: "zh_hk",
                value: "刪除成功"
            }
        ]
    },
    {
        key: "label_common_response_delete_failed",
        pairs: [
            {
                language: "en_uk",
                value: "Delete failed"
            },
            {
                language: "zh_hk",
                value: "刪除失敗"
            }
        ]
    }
]
