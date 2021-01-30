import { Master } from '../../components/master.js'
import { AGSContext } from '../../helpers/common/agsContext.js'
import { InitializePageWithMaster } from '../../helpers/common/masterHelper.js'
import { useContext, useState } from 'react'
import FunctionClaimsHelper from '../../helpers/identity/api/functionClaimsHelper.js'
import EditModal from '../../components/identity/editModal.js'
import FunctionClaimsEditModal from '../../components/identity/functionClaimsEditModal.js'
import { Table, Button } from 'reactstrap';
import axios from 'axios';
import '../../styles/identity/common.css'
import { resposne_success } from '../../config/identity.js'
import { GetLocalizedString } from '../../helpers/common/localizationHelper.js'
import {CheckIfUnauthorizedResponse} from "../../helpers/common/utilityHelper.js"

const default_functionClaim_id = "";

const defaultFunctionClaim = {
    id: default_functionClaim_id,
    name: "",
    description: []
};

export default function FunctionClaimUIWithMaster({ agsContext, pageProps }) {
    return (
        <Master agsContext={agsContext}>
            <FunctionClaimUI {...pageProps} />
        </Master>
    )
}

function FunctionClaimUI({ functionClaims }) {
    const agsContext = useContext(AGSContext);

    const [modal, setModal] = useState(false);
    const [selectedFunctionClaim, setSelectedFunctionClaim] = useState(null);

    const toggle = () => {
        setModal(!modal);
    }

    const onAddButtonClick = () => {
        setSelectedFunctionClaim(null);
        setTimeout(() => setModal(true), 0);
    }

    const onFunctionClaimSelected = (e, id) => {
        e.preventDefault();
        const clickedFunctionClaim = functionClaims.find(y => {
            return y.id == id
        })
        console.log(clickedFunctionClaim)
        setSelectedFunctionClaim(clickedFunctionClaim);
        setTimeout(() => setModal(true), 0);
    }

    const onEditModalSaveClick = async (functionClaim) => {
        // validation
        if (functionClaim.name == "" || functionClaim.name == null) {
            return "error_ags_identity_functionClaim_no_name";
        }

        const result = functionClaim.id == default_functionClaim_id ? await axios.post('/api/identity/functionClaims', { functionClaim }) : await axios.put('/api/identity/functionClaims', { functionClaim });
        return result;
    }

    const onDeleteClick = async (e, functionClaimId, functionClaimName) => {
        const confirmDelete = confirm(`${GetLocalizedString("label_identity_confirm_delete")}: ${functionClaimName}`);
        if (confirmDelete) {
            const result = await axios.delete(`/api/identity/functionClaims/${functionClaimId}`);
            if (result.data.code == resposne_success) {
                alert(GetLocalizedString("label_common_response_delete_succeeded"))
                location.reload();
            } else {
                alert(GetLocalizedString("label_common_response_delete_failed"))
            }
        }
    }

    const tbody = functionClaims == null ? (
        <div>
            <span>{GetLocalizedString("label_no_data_return")}</span>
        </div>
    ) : (
        functionClaims.map(x => {
                return (
                    <tr>
                        <td><a href="#" onClick={(e) => onFunctionClaimSelected(e, x.id)}>{x.name}</a></td>
                        <td>
                            <span class="text-theme">{x.description}</span>
                        </td>
                        <td><a href="#" onClick={(e) => onDeleteClick(e, x.id, x.name)}>{GetLocalizedString("label_common_button_delete")}</a></td>
                    </tr>
                )
            }
            )
        )


        return (
            <div>
                <div class="identity header">
                    <div>
                        <h1>{GetLocalizedString("menu_functionClaim_admin_label")}</h1>
                    </div>
                    <div>
                        <Button onClick={onAddButtonClick} >{GetLocalizedString("label_common_button_add")}</Button>
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
                        inputData={selectedFunctionClaim}
                        defaultInputData={defaultFunctionClaim}
                    >
                    </EditModal>
                </div>
            </div>
    
        )
}

export async function getServerSideProps(context) {
    const result = await InitializePageWithMaster(context.req, context.res, async () => {
        const functionClaimsHelper = new FunctionClaimsHelper(context.req, context.res);
        const functionClaimResult = await functionClaimsHelper.GetFunctionClaims();
        if (CheckIfUnauthorizedResponse(functionClaimResult)){
            return 403;
        }
        
        return {
            functionClaims: functionClaimResult.data
        }
    })

    return result;
}