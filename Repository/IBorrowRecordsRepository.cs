﻿using LMS2.Models;
using LMS2.Models.ViewModels;

namespace LMS2.Repository
{
    /// <summary>
    /// interface of Borrow Record Repo
    /// </summary>
    public interface IBorrowRecordsRepository
    {
        /// <summary>
        /// Get all Borrow Records
        /// </summary>
        /// <returns></returns>
        IQueryable<BorrowRecord> GetAllBorrowRecords();
        /// <summary>
        /// Get Borrow Record by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BorrowRecord GetBorrowRecordById(int id);
        /// <summary>
        /// Add new BorrowRecord
        /// </summary>
        /// <param name="borrowRecord"></param>
        void AddBorrowRecord(InputBorrowRecord? borrowRecord);
        /// <summary>
        /// Update Borrow Record by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inputBorrowRecord"></param>
        /// <returns></returns>
        BorrowRecord UpdateBorrowRecord(int id, InputBorrowRecord inputBorrowRecord);
        /// <summary>
        /// Delete Borrow Record By ID
        /// </summary>
        /// <param name="id"></param>
        void DeleteBorrowRecord(int id);
        /// <summary>
        /// Search borrow records over many fields
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchBorrowRecords"></param>
        /// <returns></returns>
        IQueryable<BorrowRecord> GetBorrowRecordsBySearchParams(int pageNumber, int pageSize, SearchBorrowRecords searchBorrowRecords);
        /// <summary>
        /// Get overall penalty amount by member id
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public int GetOverallPenaltyByMemberId(int memberId);
        /// <summary>
        /// Save Changes to DB
        /// </summary>
        void Save();
    }
}