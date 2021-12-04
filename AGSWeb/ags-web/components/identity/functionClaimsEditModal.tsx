import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Form, FormGroup, Label, Input, FormText, Badge } from 'reactstrap';
import { ListGroup, ListGroupItem } from 'reactstrap';
import React, { useContext, useEffect, useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTimes } from '@fortawesome/free-solid-svg-icons'
import "../../styles/identity/editModal.css"
import { GetLocalizedString } from '../../helpers/common/localizationHelper'
import { FunctionClaimEditModalProps } from '../../models/components/identity/functionClaimEditModalProps'
import { InputEventProps } from '../../models/common/inputEventProps';


export default function FunctionClaimsEditModal({editData, setEditData}: FunctionClaimEditModalProps){
    const onValueChange = (e: InputEventProps) => {
        editData[e.target.getAttribute("name")] = e.target.value;
        setEditData({ ...editData })
    }

    return (
        <React.Fragment>
            <ModalBody>
                <Form>
                    <FormGroup>
                        <Label>
                            {GetLocalizedString("label_identity_functionClaims_table_functionClaim")}:
                        </Label>
                        <Input type="text" name="name" placeholder={GetLocalizedString("label_identity_functionClaims_table_functionClaim")} value={editData.name} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            {GetLocalizedString("label_identity_functionClaims_table_description")}:
                        </Label>
                        <Input type="text" name="description" placeholder={GetLocalizedString("label_identity_functionClaims_table_description")} value={editData.description} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                </Form>
            </ModalBody>
        </React.Fragment>
    )
}