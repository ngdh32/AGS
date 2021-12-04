import { Master } from '../../components/master'
import { AGSContext } from '../../helpers/common/agsContext'
import { InitializePageWithMaster } from '../../helpers/common/masterHelper'
import { useContext, useState } from 'react'
import UsersHelper from '../../helpers/identity/api/usersHelper'
import GroupsHelper from '../../helpers/identity/api/groupsHelper'
import { Table, Button, Modal } from 'reactstrap';
import { GetLocalizedString } from '../../helpers/common/localizationHelper'
import EditModal from '../../components/identity/editModal'
import UsersEditModal from  '../../components/identity/usersEditModal'
import { resposne_success } from '../../config/identity'
import '../../styles/identity/common.css'
import axios from 'axios';
import {CheckIfUnauthorizedResponse} from "../../helpers/common/utilityHelper"
import EditModalResponse from '../../models/common/editModalResult'
import { GetServerSidePropsResult, NextApiRequest, NextApiResponse } from 'next'
import { MasterPageDataType } from '../../models/pages/masterPageDataType'
import { errorCodeNotAuthenticated, errorCodeSuccess } from '../../config/common'
import { UserItemType } from '../../models/identity/userItemType'
import { GroupItemType } from '../../models/identity/groupItemType'
import EditModalResult from '../../models/common/editModalResult'
import { UserEditModalProps } from '../../models/components/identity/userEditModalProps'
import AGSResponse from '../../models/common/agsResponse'

const default_user_id = "";

const defaultUser = {
    id: default_user_id,
    username: '',
    email: '',
    first_Name: '',
    last_Name: '',
    title: '',
    groupIds: Array<string>()
};

export default function UsersUIWithMaster({ agsContext, pageData }: MasterPageDataType) {
    return (
        <Master agsContext={agsContext}>
            <UsersUI {...pageData} />
        </Master>
    )
}

function UsersUI({ users, groups }: { users: UserItemType[], groups: GroupItemType[] }) {
    const agsContext = useContext(AGSContext);
    const [modal, setModal] = useState(false);
    const [selectedUser, setSelectedUser] = useState(null);

    const toggle = () => {
        setModal(!modal);
    }
    
    const onAddButtonClick = () => {
        setSelectedUser(null);
        setTimeout(() => setModal(true), 0);
    }

    const onUserSelected = (e: React.MouseEvent<HTMLElement>, id: string) => {
        e.preventDefault();
        const clickedUser = users.find(y => {
            return y.id == id
        })
        console.log(clickedUser)
        setSelectedUser(clickedUser);
        setTimeout(() => setModal(true), 0);
    }

    const onEditModalSaveClick = async (user: UserItemType) : Promise<EditModalResponse> => {
        const response = new EditModalResponse();
        // validation
        if (user.username == "" || user.username == null){
            return EditModalResult.SetUnsuccessfulResponseWithErrorMessage("error_ags_identity_users_no_username");
        }

        const result = user.id == default_user_id ? await axios.post('/api/identity/users', { user }) : await axios.put('/api/identity/users', { user });
        return EditModalResult.SetSuccessfulResponse();
    }

    const onDeleteClick = async (e: React.MouseEvent<HTMLElement>, userId: string, username: string) => {
        const confirmDelete = confirm(`${GetLocalizedString("label_identity_confirm_delete")}: ${username}`);
        if (confirmDelete){
            const result:AGSResponse = await axios.delete(`/api/identity/users/${userId}`);
            if (result.data.isSuccessful){
                alert(GetLocalizedString("label_common_response_delete_succeeded"))
                location.reload();
            } else {
                alert(GetLocalizedString("label_common_response_delete_failed"))
            }
        }
    }
    
    const tbody = users == null ? (
        <div>
            <span>{GetLocalizedString("label_no_data_return")}</span>
        </div>
    ) : (

            users.map(x => {
                return (
                    <tr>
                        <td><a href="#" onClick={(e) => onUserSelected(e, x.id)}>{x.username}</a></td>
                        <td>{x.email}</td>
                        <td>{x.firstName}</td>
                        <td>{x.lastName}</td>
                        <td>{x.title}</td>
                        <td><a href="#" onClick={(e) => onDeleteClick(e, x.id, x.username)}>{GetLocalizedString("label_common_button_delete")}</a></td>
                    </tr>
                )
            }
            )
        )

    return (
        <div>
            <div className="identity header">
                <div>
                    <h1>{GetLocalizedString("menu_user_admin_label")}</h1>
                </div>
                <div>
                    <Button color="primary" onClick={onAddButtonClick} >{GetLocalizedString("label_common_button_add")}</Button>
                </div>
            </div>
            <Table hover>
                <thead>
                    <tr>
                        <th>{GetLocalizedString("label_identity_user_table_username")}</th>
                        <th>{GetLocalizedString("label_identity_user_table_email")}</th>
                        <th>{GetLocalizedString("label_identity_user_table_first_name")}</th>
                        <th>{GetLocalizedString("label_identity_user_table_last_name")}</th>
                        <th>{GetLocalizedString("label_identity_user_table_title")}</th>
                        <th>{GetLocalizedString("label_common_button_action")}</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        tbody
                    }
                </tbody>
            </Table>
            <div>
                <EditModal 
                    isOpen={modal} 
                    toggle={toggle} 
                    title={ selectedUser == null ? GetLocalizedString("label_identity_user_create_modal_title") : `${GetLocalizedString("label_identity_user_table_username")}: ${selectedUser.username}` }
                    onSaveClick={onEditModalSaveClick}
                    ConcreteEditModal={UsersEditModal}
                    concreteEditModalProps={{groups}}
                    inputData={selectedUser}
                    defaultInputData={defaultUser}
                    />
            </div>
        </div>

    )
}

export async function getServerSideProps(context: { req: NextApiRequest; res: NextApiResponse; }) : Promise<GetServerSidePropsResult<MasterPageDataType>> {
    const result = await InitializePageWithMaster(context.req, context.res, async () => {
        const usersHelper = new UsersHelper(context.req, context.res)
        const usersResult = await usersHelper.GetUsers();
        const groupsHelper = new GroupsHelper(context.req, context.res);
        const groupResult = await groupsHelper.GetGroups();

        if (CheckIfUnauthorizedResponse(usersResult) || CheckIfUnauthorizedResponse(groupResult)){
            return {
                errorCode: errorCodeNotAuthenticated,
                pageData: null
            }
        }

        return {
            errorCode: errorCodeSuccess,
            pageData: {
                users: usersResult.data
                , groups: groupResult.data
            }
        }
    })

    return result;
}
