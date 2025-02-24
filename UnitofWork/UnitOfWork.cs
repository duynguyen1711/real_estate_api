﻿using Microsoft.EntityFrameworkCore;
using real_estate_api.Data;
using real_estate_api.Interface.Repository;
using real_estate_api.Repositories;
using System;

namespace real_estate_api.UnitofWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IUserRepository _userRepository;
        private IPostRepository _postRepository;
        private IPostDetailRepository _postDetailRepository;
        private ISavedPostRepository _savedPostRepository;
        private IChatRepository _chatRepository;
        private IMessageRepository _messageRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
        public IPostRepository PostRepository => _postRepository ??= new PostRepository(_context);
        public IPostDetailRepository PostDetailRepository => _postDetailRepository ??= new PostDetailRepository(_context);
        public ISavedPostRepository SavedPostRepository => _savedPostRepository ??= new SavedPostRepository(_context);
        public IChatRepository ChatRepository => _chatRepository ??= new ChatRepository(_context);
        public IMessageRepository MessageRepository => _messageRepository ??= new MessageRepository(_context);


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
