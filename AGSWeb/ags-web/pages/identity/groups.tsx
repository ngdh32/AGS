import { Master } from '../../components/master'
import { AGSContext } from '../../helpers/common/agsContext'
import { InitializePageWithMaster } from '../../helpers/common/masterHelper'
import { useContext, useState } from 'react'
import GroupsHelper from '../../helpers/identity/api/groupsHelper'
import FunctionClaimsHelper from '../../helpers/identity/api/functionClaimsHelper'
import { Table, Button, Modal } from 'reactstrap';
import { GetLocalizedString } from '../../helpers/common/localizationHelper'
import EditModal from '../../components/identity/editModal'
import GroupsEditModal from '../../components/identity/groupsEditModal'
import '../../styles/identity/common.css'
import axios from 'axios';
import { resposne_success } from '../../config/identity'
import {CheckIfUnauthorizedResponse} from "../../helpers/common/utilityHelper"
import { GetServerSidePropsResult, NextApiRequest, NextApiResponse } from 'next'
import { MasterPageDataType } from '../../models/pages/masterPageDataType'
import { errorCodeNotAuthenticated, errorCodeSuccess } from '../../config/common'
import { GroupItemType } from '../../models/identity/groupItemType'
import { FunctionClaimItemType } from '../../models/identity/functionClaimItemType'
import EditModalResult from '../../models/common/editModalResult'

const default_group_id = "";

const defaultGroup = {
    id: default_group_id,
    name: "",
    functionClaimIds:  Array<string> ()
};

export default function GroupUIWithMaster({ agsContext, pageData }: MasterPageDataType) {
    return (
        <Master agsContext={agsContext}>
            <GroupUI {...pageData} />
        </Master>
    )
}

function GroupUI({ groups, functionClaims }: { groups: GroupItemType[], functionClaims: FunctionClaimItemType[]}) {
    const agsContext = useContext(AGSContext);
    const [modal, setModal] = useState(false);
    const [selectedGroup, setSelectedGroup] = useState(null);

    const toggle = () => {
        setModal(!modal);
    }

    const onAddButtonClick = () => {
        setSelectedGroup(null);
        setTimeout(() => setModal(true), 0);
    }

    const onGroupSelected = (e: React.MouseEvent<HTMLElement>, id: string) => {
        e.preventDefault();
        const clickedGroup = groups.find(y => {
            return y.id == id
        })
        console.log(clickedGroup)
        setSelectedGroup(clickedGroup);
        setTimeout(() => setModal(true), 0);
    }

    const onEditModalSaveClick = async (group: GroupItemType)  : Promise<EditModalResult> => {
        // validation
        if (group.name == "" || group.name == null) {
            return EditModalResult.SetUnsuccessfulResponseWithErrorMessage("error_ags_identity_group_no_name");
        }

        const result = group.id == default_group_id ? await axios.post('/api/identity/groups', { group }) : await axios.put('/api/identity/groups', { group });
        return EditModalResult.SetSuccessfulResponse();
    }

    const onDeleteClick = async (e: React.MouseEvent<HTMLElement>, groupId: string, groupName: string) => {
        const confirmDelete = confirm(`${GetLocalizedString("label_identity_confirm_delete")}: ${groupName}`);
        if (confirmDelete) {
            const result = await axios.delete(`/api/identity/groups/${groupId}`);
            if (result.data.code == resposne_success) {
                alert(GetLocalizedString("label_common_response_delete_succeeded"))
                location.reload();
            } else {
                alert(GetLocalizedString("label_common_response_delete_failed"))
            }
        }
    }

    const tbody = groups == null ? (
        <div>
            <span>{GetLocalizedString("label_no_data_return")}</span>
        </div>
    ) : (

            groups.map(x => {
                return (
                    <tr>
                        <td><a href="#" onClick={(e) => onGroupSelected(e, x.id)}>{x.name}</a></td>
                        <td>
                            <span>
                                {
                                    x.functionClaimIds.map(y => {
                                        return functionClaims.find(z => y == z.id).name
                                    }).join(",")
                                }
                            </span>
                        </td>
                        <td><a href="#" onClick={(e) => onDeleteClick(e, x.id, x.name)}>{GetLocalizedString("label_common_button_delete")}</a></td>
                    </tr>
                )
            }
            )
        )

    return (
        <div>
            <div className="identity header">
                <div>
                    <h1>{GetLocalizedString("menu_group_admin_label")}</h1>
                </div>
                <div>
                    <Button color="primary" onClick={onAddButtonClick} >{GetLocalizedString("label_common_button_add")}</Button>
                </div>
            </div>
            <Table hover>
                <thead>
                    <tr>
                        <th>{GetLocalizedString("label_identity_group_table_name")}</th>
                        <th>{GetLocalizedString("label_identity_group_table_functionClaim")}</th>
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
                    title={selectedGroup == null ? GetLocalizedString("label_identity_group_create_modal_title") : `${GetLocalizedString("label_identity_group_table_name")}: ${selectedGroup.name}`}
                    onSaveClick={onEditModalSaveClick}
                    concreteEditModal={GroupsEditModal}
                    concreteEditModalProps={{ functionClaims }}
                    inputData={selectedGroup}
                    defaultInputData={defaultGroup}
                />
            </div>
        </div>

    )
}

export async function getServerSideProps(context: { req: NextApiRequest; res: NextApiResponse; }) : Promise<GetServerSidePropsResult<MasterPageDataType>> {
    const result = await InitializePageWithMaster(context.req, context.res, async () => {
        const groupsHelper = new GroupsHelper(context.req, context.res);
        const groupsResult = await groupsHelper.GetGroups();
        const functionClaimsHelper = new FunctionClaimsHelper(context.req, context.res);
        const functionClaimsResult = await functionClaimsHelper.GetFunctionClaims();
        
        if (CheckIfUnauthorizedResponse(groupsResult) || CheckIfUnauthorizedResponse(functionClaimsResult)){
            return {
                errorCode: errorCodeNotAuthenticated,
                pageData: null
            }
        }

        return {
            errorCode: errorCodeSuccess,
            pageData: {
                    groups: groupsResult.data,
                    functionClaims: functionClaimsResult.data
            }
        }
    })

    
    return result;
}