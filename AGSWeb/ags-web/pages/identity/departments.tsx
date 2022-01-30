import { Master } from '../../components/master'
import { AGSContext } from '../../helpers/common/agsContext'
import { InitializePageWithMaster } from '../../helpers/common/masterHelper'
import { useContext, useState } from 'react'
import FunctionClaimsHelper from '../../helpers/identity/api/functionClaimsHelper'
import EditModal from '../../components/identity/editModal'
import FunctionClaimsEditModal from '../../components/identity/functionClaimsEditModal'
import { Table, Button } from 'reactstrap';
import axios from 'axios';
import '../../styles/identity/common.css'
import { resposne_success } from '../../config/identity'
import { GetLocalizedString } from '../../helpers/common/localizationHelper'
import { CheckIfUnauthorizedResponse } from "../../helpers/common/utilityHelper"
import { NextApiRequest, NextApiResponse } from 'next'
import { GetServerSidePropsResult } from 'next'
import { MasterPageDataType } from '../../models/pages/masterPageDataType'
import { errorCodeSuccess, errorCodeNotAuthenticated } from '../../config/common'
import { FunctionClaimItemType } from '../../models/identity/functionClaimItemType'
import EditModalResult from '../../models/common/editModalResult'
import AGSResponse from '../../models/common/agsResponse'
import { DepartmentItemType } from '../../models/identity/departmentItemType'
import { UserItemType } from '../../models/identity/userItemType'

const default_department_id = "";

const defaultFunctionClaim: DepartmentItemType = {
    id: default_department_id,
    name: "",
    headUserId: null,
    parentDepartmentId: null,
    userIds: Array<string>()
};

export default function DepartmentUIWithMaster({ agsContext, pageData }: MasterPageDataType) {
    return (
        <Master agsContext={agsContext}>
            <DepartmentUI {...pageData} />
        </Master>
    )
}

function DepartmentUI({ departments, users }: { departments: DepartmentItemType[], users: UserItemType[] }) {
    const agsContext = useContext(AGSContext);

    const [modal, setModal] = useState(false);
    const [selectedFunctionClaim, setSelectedDepartment] = useState(null);

    const toggle = () => {
        setModal(!modal);
    }

    const onAddButtonClick = () => {
        setSelectedDepartment(null);
        setTimeout(() => setModal(true), 0);
    }

    const onDepartmentSelected = (e: React.MouseEvent<HTMLElement>, id: string) => {
        e.preventDefault();
        const clickedDepartment = departments.find(y => {
            return y.id == id
        })
        console.log(clickedDepartment)
        setSelectedDepartment(clickedDepartment);
        setTimeout(() => setModal(true), 0);
    }

    const onEditModalSaveClick = async (department: DepartmentItemType): Promise<EditModalResult> => {
        // validation
        if (department.name == "" || department.name == null) {
            return EditModalResult.SetUnsuccessfulResponseWithErrorMessage("error_ags_identity_department_no_name");
        }

        const result = department.id == default_department_id ? await axios.post('/api/identity/departments', { department }) : await axios.put('/api/identity/departments', { department });
        return EditModalResult.SetSuccessfulResponse();
    }

    const onDeleteClick = async (e: React.MouseEvent<HTMLElement>, departmentId: string, departmentName: string) => {
        const confirmDelete = confirm(`${GetLocalizedString("label_identity_confirm_delete")}: ${departmentName}`);
        if (confirmDelete) {
            const result: AGSResponse = await axios.delete(`/api/identity/departments/${departmentId}`);
            if (result.data.isSuccessful) {
                alert(GetLocalizedString("label_common_response_delete_succeeded"))
                location.reload();
            } else {
                alert(GetLocalizedString("label_common_response_delete_failed"))
            }
        }
    }

    const tbody = departments == null ? (
        <div>
            <span>{GetLocalizedString("label_no_data_return")}</span>
        </div>
    ) : (
            departments.map(x => {
                const headUser = users.find(y => y.id == x.headUserId);
                const parentDepartment = departments.find(z => z.id == x.parentDepartmentId);
                const deparmentUsers = users.flat
                return (

                    <tr>
                        <td><a href="#" onClick={(e) => onDepartmentSelected(e, x.id)}>{x.name}</a></td>
                        <td>
                            <span className="text-theme">{headUser?.username ?? ""}</span>
                        </td>
                        <td>
                            <span className="text-theme">{parentDepartment?.name ?? ""}</span>
                        </td>
                        <td>
                            <span className="text-theme">{x.userIds}</span>
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
                    <h1>{GetLocalizedString("menu_functionClaim_admin_label")}</h1>
                </div>
                <div>
                    <Button color="primary" onClick={onAddButtonClick} >{GetLocalizedString("label_common_button_add")}</Button>
                </div>
            </div>
            <Table hover>
                <thead>
                    <tr>
                        <th>{GetLocalizedString("label_identity_functionClaims_table_functionClaim")}</th>
                        <th>{GetLocalizedString("label_identity_functionClaims_table_description")}</th>
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
                    title={selectedFunctionClaim == null ? GetLocalizedString("label_identity_functionClaims_create_modal_title") : `${GetLocalizedString("label_identity_functionClaims_table_functionClaim")}: ${selectedFunctionClaim.name}`}
                    onSaveClick={onEditModalSaveClick}
                    ConcreteEditModal={FunctionClaimsEditModal}
                    concreteEditModalProps={null}
                    inputData={selectedFunctionClaim}
                    defaultInputData={defaultFunctionClaim}
                />
            </div>
        </div>

    )
}

export async function getServerSideProps(context: { req: NextApiRequest; res: NextApiResponse; }): Promise<GetServerSidePropsResult<MasterPageDataType>> {
    const result = await InitializePageWithMaster(context.req, context.res, async () => {
        const functionClaimsHelper = new FunctionClaimsHelper(context.req, context.res);
        const functionClaimResult = await functionClaimsHelper.GetFunctionClaims();

        if (CheckIfUnauthorizedResponse(functionClaimResult)) {
            return {
                errorCode: errorCodeNotAuthenticated,
                pageData: null
            }
        }

        return {
            errorCode: errorCodeSuccess,
            pageData: {
                functionClaims: functionClaimResult.data
            }
        }
    })

    return result;
}