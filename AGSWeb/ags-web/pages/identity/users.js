import { Master } from '../../components/Master.js'
import { AGSContext } from '../../helpers/common/agsContext.js'
import { InitializePageWithMaster } from '../../helpers/common/masterHelper.js'
import { useContext, useEffect, useState } from 'react'
import UsersHelper from '../../helpers/identity/api/usersHelper.js'
import GroupsHelper from '../../helpers/identity/api/groupsHelper.js'
import { Table, Button, Modal } from 'reactstrap';
import { GetLocalizedString } from '../../helpers/common/localizationHelper.js'
import EditModal from '../../components/identity/editModal.js'
import UsersEditModal from  '../../components/identity/usersEditModal.js'
import GroupsEditModal from  '../../components/identity/groupsEditModal.js'
import '../../styles/identity/common.css'
import axios from 'axios';

export default function GroupUIWithMaster({ agsContext, pageProps }) {
    return (
        <Master agsContext={agsContext}>
            <GroupUI {...pageProps} />
        </Master>
    )
}

function GroupUI({ users, groups }) {
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

    const onUserSelected = (e, id) => {
        e.preventDefault();
        const clickedUser = users.find(y => {
            return y.id == id
        })
        console.log(clickedUser)
        setSelectedUser(clickedUser);
        setTimeout(() => setModal(true), 0);
    }

    const onEditModalSaveClick = async (user) => {
        // validation
        if (user.username == "" || user.username == null){
            return "error_ags_identity_users_no_username";
        }

        const result = await axios.post('/api/identity/users', { user })
        return result;
    }
    
    const defaultUser = {
        id: '',
        username: '',
        email: '',
        first_Name: '',
        last_Name: '',
        title: '',
        groupIds: []
    };
    

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
                        <td>{x.first_Name}</td>
                        <td>{x.last_Name}</td>
                        <td>{x.title}</td>
                    </tr>
                )
            }
            )
        )

    return (
        <div>
            <div class="identity header">
                <div>
                    <h1>{GetLocalizedString("menu_user_admin_label")}</h1>
                </div>
                <div>
                    <Button onClick={onAddButtonClick} >{GetLocalizedString("label_common_button_add")}</Button>
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
                    title={ selectedUser == null ? "Create User" : `User: ${selectedUser.username}` }
                    onSaveClick={onEditModalSaveClick}
                    ConcreteEditModal={UsersEditModal}
                    concreteEditModalProps={{groups}}
                    inputData={selectedUser}
                    defaultInputData={defaultUser}
                    >
                </EditModal>
            </div>
        </div>

    )
}

export async function getServerSideProps(context) {
    const result = await InitializePageWithMaster(context.req, context.res, async () => {
        const usersHelper = new UsersHelper(context.req, context.res)
        const usersResult = await usersHelper.GetUsers();
        const groupsHelper = new GroupsHelper(context.req, context.res);
        const groups = await groupsHelper.GetGroups();


        return {
            users: usersResult.data
            , groups
        }
    })

    return result;
}
