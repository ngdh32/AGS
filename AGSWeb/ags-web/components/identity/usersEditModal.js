import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Form, FormGroup, Label, Input, FormText, Badge } from 'reactstrap';
import { ListGroup, ListGroupItem } from 'reactstrap';
import React, { useContext, useEffect, useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTimes } from '@fortawesome/free-solid-svg-icons'
import "../../styles/identity/userEditModal.css"

export default function UsersEditModal({ toggle, selectedUser, groups }) {
    const isCreate = selectedUser == null ? true : false;
    const defaultUser = {
        id: '',
        username: '',
        email: '',
        first_Name: '',
        last_Name: '',
        title: '',
        groupIds: []
    };
    const [user, setUser] = useState(selectedUser == null ? defaultUser : JSON.parse(JSON.stringify(selectedUser)));
    const [filteredGroups, setFilteredGroups] = useState(groups);
    const [showGroupOptions, setShowGroupOptions] = useState(false);

    const onValueChange = (e) => {
        user[e.target.getAttribute("name")] = e.target.value;
        setUser({ ...user })
    }

    const onGroupInputChange = (input) => {
        let groupOptions = input == null || input == "" ? groups : groups.filter(x => {
            return x.name.toUpperCase().includes(input.toUpperCase());
        });

        groupOptions = groupOptions.filter(x => {
            const isAdded = user.groupIds.some(y => {
                return y == x.id
            })
            return isAdded ? false : true;
        })

        setFilteredGroups(groupOptions);
        setTimeout(() => setShowGroupOptions(true), 0);
    }

    const onModalClick = (e) => {
        const groupList = document.getElementById("groupList")
        if (e.target != groupList) {
            setShowGroupOptions(false)
        }
    }

    const onGroupOptionClick = (groupId) => {
        user.groupIds.push(groupId);
        setUser({ ...user })
        setShowGroupOptions(false)
    }

    const onGroupBadgeRemoveClick = (groupId) => {
        const updatedGroupIds = []; 
        user.groupIds.forEach(x => {
            if (x != groupId){
                updatedGroupIds.push(x);
            }
        })
        console.log(updatedGroupIds)
        
        user.groupIds = updatedGroupIds;
        setUser({...user})
    }

    const selectedGroupBadges = user.groupIds.map(x => {
        const group = groups.find(y => {return y.id == x});
        return (
            <GroupBadge group={group} onGroupBadgeRemoveClick={onGroupBadgeRemoveClick}/>
        )
    })

    return (
        <React.Fragment>
            <ModalHeader toggle={toggle}>
                {user.username == null ? "Create user" : `User: ${user.username}`}
            </ModalHeader>
            <ModalBody onClick={onModalClick}>
                <Form>
                    <FormGroup>
                        <Label>
                            Username:
                        </Label>
                        <Input type="text" name="username" placeholder="Username" value={user.username} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            Email:
                        </Label>
                        <Input type="email" name="email" placeholder="Email" value={user.email} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            First Name:
                        </Label>
                        <Input type="text" name="first_Name" placeholder="First Name" value={user.first_Name} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            Last Name:
                        </Label>
                        <Input type="text" name="last_Name" placeholder="Last Name" value={user.last_Name} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            Title:
                        </Label>
                        <Input type="text" name="title" placeholder="Title" value={user.title} onChange={(e) => onValueChange(e)} />
                    </FormGroup>
                    <FormGroup>
                        <Label>
                            Groups:
                        </Label>
                        <div class="form-control user-edit groups">
                            {
                                selectedGroupBadges
                            }
                            <div>
                                <Input className="input" type="text" onClick={() => onGroupInputChange(null)} onChange={(e) => { onGroupInputChange(e.target.value) }}></Input>
                                <div id="groupList">
                                    <ListGroup className={"groupList " + (showGroupOptions ? "" : "hide")}>
                                        {
                                            filteredGroups.map(x => {
                                                return (
                                                    <ListGroupItem className="groupOption" action onClick={() => onGroupOptionClick(x.id)}>
                                                        {x.name}
                                                    </ListGroupItem>
                                                )
                                            })
                                        }
                                    </ListGroup>
                                </div>
                            </div>
                        </div>
                    </FormGroup>
                </Form>
            </ModalBody>
            <ModalFooter>
                <Button color="primary" onClick={toggle}>Save</Button>
                <Button color="secondary" onClick={toggle}>Cancel</Button>
            </ModalFooter>
        </React.Fragment>
    )
}

function GroupBadge({group, onGroupBadgeRemoveClick}) {
    return (
        <React.Fragment>
            <Button className="groupBadge" color="secondary" type="button">{group.name} <FontAwesomeIcon icon={faTimes} onClick={() => onGroupBadgeRemoveClick(group.id)}></FontAwesomeIcon></Button>
        </React.Fragment>
    )
}