import React, { useState, useEffect, useContext, useCallback, useRef } from 'react';
import api from '../../services/api';
import { VscCalendar } from "react-icons/vsc";
import PostForm from '../../components/PostForm/PostForm';
import Header from '../../components/Header/Header';
import Post from '../../components/Post/Post';
import { UserContext } from '../../contexts/userContext';
import { useParams } from 'react-router-dom';
import { showSuccessMessage, showErrorMessage } from '../../utils/sweetalertUtils';

import './profile.css'

const Profile = () => {
    const defaultUser = {
        id: 0,
        name: null,
        username: null,
    };
    const { id } = useParams();
    const [posts, setPosts] = useState([]);
    const [newPostContent, setNewPostContent] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [filter, setFilter] = useState('');
    const [sortOrder, setSortOrder] = useState('latest');
    const [profileUser, setProfileUser] = useState(defaultUser);
    const { selectedUser } = useContext(UserContext);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPosts, setTotalPosts] = useState(0); 
    const observer = useRef();

    const getUserById = useCallback(async () => {
        setLoading(true);
        try {
            const res = await api.get(`/users/${id}`);
            setProfileUser(res.data);
            setLoading(false);
        } catch (error) {
            setError(error.message);
            setLoading(false);
        }
    }, [id]);

    useEffect(() => {
        if (id) {
            getUserById();
        }
    }, [id, getUserById]);

    useEffect(() => {
        if (profileUser.id !== 0) {
            fetchPosts(1, 10);
        }
    }, [profileUser.id, sortOrder, filter]);

    const fetchPosts = async (page, take) => {
        setLoading(true);
        try {
            const res = await api.get('/posts', {
                params: {
                    skip: (page - 1) * take,
                    take: take,
                    sortBy: sortOrder,
                    keyword: filter,
                    userId: profileUser.id
                }
            });
            setPosts(prevPosts => (page === 1 ? res.data.posts : [...prevPosts, ...res.data.posts]));
            setCurrentPage(page);
            setTotalPosts(res.totalPosts);
            setLoading(false);
        } catch (error) {
            setError(error.message);
            setLoading(false);
            showErrorMessage(`Failed to fetch posts: ${error.message}`);
        }
    };

    const handleCreatePost = async () => {
        const post = { content: newPostContent, userId: selectedUser.id };
        try {
            await api.post('/posts', post);
            setNewPostContent('');
            showSuccessMessage('Post created successfully!');
            fetchPosts(1, posts.length + 1);
        } catch (error) {
            showErrorMessage(`Failed to create post: ${error.response.data}`);
        }
    };

    // Infinite scrolling observer
    const lastPostElementRef = useCallback(node => {
        if (loading) return; 
        if (observer.current) observer.current.disconnect();
        observer.current = new IntersectionObserver(entries => {
            if (entries[0].isIntersecting && posts.length < totalPosts) {
                fetchPosts(currentPage + 1, 10); 
            }
        });
        if (node) observer.current.observe(node);
    }, [loading, currentPage, totalPosts]);

    return (
        <div>
            <Header />
            <div className="user-page-container">
                <img src="/user_default.png" alt="logged-user-avatar" />

                <div className="user-data-container">
                    <div className="user-data-container-2">
                        <span className="user-name">{profileUser.username}</span>

                        <div className="user-username-container">
                            <span className="user-info-description">{profileUser.name}</span>
                        </div>

                        <div className="user-info-container">
                            <span className="user-info-description">
                                <VscCalendar />
                                {` Joined ${new Date(profileUser.createdAt).toLocaleDateString()}`}
                            </span>
                        </div>
                        <div className="user-info-container">
                            <span className="user-info-value">
                                {profileUser.totalPosts ?? 0}
                            </span>
                            <span className="user-info-description">Total posts</span>
                        </div>
                    </div>
                </div>
            </div>

            {profileUser && selectedUser && profileUser.id === selectedUser.id ? (
                <div className="user-post-form-container">
                    <PostForm
                        newPostContent={newPostContent}
                        handleCreatePost={handleCreatePost}
                        updateNewPostContent={setNewPostContent}
                    />
                </div>
            ) : null}

            <div className="user-posts-container">
                {loading && posts.length === 0 ? (
                    <p>Loading...</p>
                ) : error ? (
                    <p>Error: {error}</p>
                ) : (
                    <div>
                        {posts.map((post, index) => {
                            if (posts.length === index + 1) {
                                return (
                                    <div key={post.id} className="post-wrapper" ref={lastPostElementRef}>
                                        <Post post={post} />
                                    </div>
                                );
                            } else {
                                return (
                                    <div key={post.id} className="post-wrapper">
                                        <Post post={post} />
                                    </div>
                                );
                            }
                        })}
                    </div>
                )}
                {loading && posts.length !== 0 && <p>Loading more posts...</p>}
            </div>
        </div>
    );
};

export default Profile;
